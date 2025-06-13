using Data;
using Exceptions;
using Microsoft.EntityFrameworkCore;
using Modules.Category.Dto;
using Modules.Category.Model;
using Modules.News.Dto;

namespace Modules.Category.Service;

public class CategoryService(AppDbContext context) : BaseService(context)
{
    public async Task<List<NewsOnCategoryDto>> GetManyNewsOnCategoryId(Guid id)
    {
        bool exists = await context.Categories.AnyAsync(c => c.Id == id && c.Active);
        if (!exists)
            throw new CategoryNotFoundException(id);

        return await context
            .News.Where(n => n.CategoryId == id && n.Active)
            .Include(n => n.Author)
            .Include(n => n.Category)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NewsOnCategoryDto
            {
                Title = n.Title,
                Id = n.Id,
                CreatedAt = n.CreatedAt,
            })
            .ToListAsync();
    }

    public async Task<CategoryModel> GetCategory(Guid id) =>
        await context.Categories.Where(c => c.Active).FirstOrDefaultAsync(c => c.Id == id)
        ?? throw new CategoryNotFoundException(id);

    public async Task<List<CategoryDto>> GetAllCategories() =>
        await context
            .Categories.Where(c => c.Active)
            .Select(c => new CategoryDto { Name = c.Name, Id = c.Id })
            .ToListAsync() ?? throw new CategoryNotFoundException(null);

    public async Task<CategoryModel> Create(CategoryModel category)
    {
        bool exists = await context.Categories.AnyAsync(c => c.Name == category.Name && c.Active);
        if (exists)
            throw new ConflictCategoryException(category.Name);

        context.Categories.Add(category);

        await SaveAsync();

        return category;
    }

    public async Task Inactive(Guid id)
    {
        CategoryModel findCategory =
            await context.Categories.FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new CategoryNotFoundException(id);
        findCategory.Active = false;
        findCategory.UpdatedAt = DateTime.UtcNow;

        context.Categories.Update(findCategory);
        await SaveAsync();
    }
}
