using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Modules.Category;

public class CategoryService(AppDbContext context)
{
    public async Task<List<NewsDto>> GetManyNewsOnCategoryId(Guid id)
    {
        return await context
            .News.Include(n => n.Author)
            .Include(n => n.Category)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NewsDto
            {
                Title = n.Title,
                Description = n.Description,
                Thumbnail = n.Thumbnail,
                Author = new AuthorDto
                {
                    Name = n.Author!.Name,
                    Role = n.Author.Role,
                    Id = n.Author.Id,
                },
                Category = new CategoryDto { Name = n.Category!.Name, Id = n.Category.Id },
                Id = n.Id,
                Active = n.Active,
                CreatedAt = n.CreatedAt,
            })
            .ToListAsync();
    }

    public async Task<CategoryModel?> Create(CategoryModel category)
    {
        context.Categories.Add(category);
        var saved = await context.SaveChangesAsync();
        return saved > 0 ? category : null;
    }
}
