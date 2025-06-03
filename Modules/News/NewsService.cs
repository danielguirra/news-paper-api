using Data;
using Exceptions;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Modules.News
{
    public class NewsService(AppDbContext context) : BaseService(context)
    {
        public async Task<List<NewsRecentsDto>> Recents(int take = 10, int skip = 0)
        {
            if (take <= 0 || skip < 0)
                throw new BadRequestTakeSkip();
            var recents = await context
                .News.Include(n => n.Author)
                .Include(n => n.Category)
                .Where(n => n.Active)
                .OrderByDescending(n => n.CreatedAt)
                .Skip(skip)
                .Take(take)
                .Select(n => new NewsRecentsDto
                {
                    Title = n.Title,
                    Description = n.Description,
                    Thumbnail = n.Thumbnail,
                    Id = n.Id,
                    Active = n.Active,
                    CreatedAt = n.CreatedAt,
                })
                .ToListAsync();

            if (recents.Count == 0)
                throw new NewsNotFoundException(null);

            return recents;
        }

        public async Task<NewsDto> GetOneById(Guid id)
        {
            var news = await context
                .News.Where(n => n.Active)
                .Include(n => n.Author)
                .Include(n => n.Category)
                .Select(n => new NewsDto
                {
                    Title = n.Title,
                    Description = n.Description,
                    Content = n.Content,
                    Thumbnail = n.Thumbnail,
                    Author = new AuthorDto
                    {
                        Name = n.Author!.Name,
                        Role = n.Author!.Role,
                        Id = n.AuthorId,
                    },
                    Category = new CategoryDto { Name = n.Category!.Name, Id = n.Category.Id },
                    Id = n.Id,
                    Active = n.Active,
                    CreatedAt = n.CreatedAt,
                    UpdatedAt = n.UpdatedAt,
                })
                .FirstOrDefaultAsync(n => n.Id == id);

            return news ?? throw new NewsNotFoundException(id);
        }

        public async Task<List<NewsRecentsDto>> GetByTitle(string title)
        {
            var news = await context
                .News.Include(n => n.Author)
                .Where(n => EF.Functions.ILike(n.Title, $"%{title}%") && n.Active)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NewsRecentsDto
                {
                    Title = n.Title,
                    Description = n.Description,
                    Thumbnail = n.Thumbnail,
                    Id = n.Id,
                    Active = n.Active,
                    CreatedAt = n.CreatedAt,
                })
                .ToListAsync();

            if (news.Count == 0)
                throw new NewsNotFoundException(null);

            return news;
        }

        public async Task<NewsModel> Create(NewsModel news)
        {
            if (!await AliveCategory(news.CategoryId))
                throw new InvalidNewsCategoryException(news.CategoryId);

            if (await Exists(news.Title))
                throw new ConflictNewsException(news.Title);

            context.News.Add(news);
            await SaveAsync();

            return news;
        }

        public async Task<bool> AliveCategory(Guid id) =>
            await context.Categories.AnyAsync(c => c.Id == id);

        public async Task<bool> Exists(string title) =>
            await context.News.AnyAsync(n => n.Title == title);

        public async Task Edit(EditNewsDto dto)
        {
            var findNews = await GetNewsOrThrow(dto.Id);

            if (!string.IsNullOrWhiteSpace(dto.Title))
                findNews.Title = dto.Title;

            if (!string.IsNullOrWhiteSpace(dto.Description))
                findNews.Description = dto.Description;

            if (!string.IsNullOrWhiteSpace(dto.Content))
                findNews.Content = dto.Content;

            if (!string.IsNullOrWhiteSpace(dto.Thumbnail))
                findNews.Thumbnail = dto.Thumbnail;

            if (dto.Active.HasValue)
                findNews.Active = dto.Active.Value;

            if (dto.CategoryId.HasValue)
                findNews.CategoryId = dto.CategoryId.Value;

            findNews.UpdatedAt = DateTime.UtcNow;

            context.News.Update(findNews);
            await SaveAsync();
        }

        public async Task Inactive(Guid id)
        {
            var news = await GetNewsOrThrow(id);

            news.Active = false;
            context.News.Update(news);

            await SaveAsync();
        }

        // Não implementado, apenas teste
        public async Task Clean()
        {
            var news = await context.News.ToListAsync();
            context.News.RemoveRange(news);
            await SaveAsync();
        }

        private async Task<NewsModel> GetNewsOrThrow(Guid id)
        {
            var news = await context.News.FirstOrDefaultAsync(n => n.Id == id);
            return news ?? throw new NewsNotFoundException(id);
        }
    }
}
