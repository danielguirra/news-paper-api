using Data;

using Dto;

using Microsoft.EntityFrameworkCore;

using Models;

namespace Services
{
  public class NewsService(AppDbContext context)
  {
    public async Task<List<NewsRecentsDto>> Recents(int take = 10, int skip = 0)
    {
      return await context.News
          .Include(n => n.Author)
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
    }


    public async Task<NewsDto?> GetOneById(Guid id) =>
      await context.News.Select(n => new NewsDto
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
        Id = n.Id,
        Active = n.Active,
        CreatedAt = n.CreatedAt,
        UpdatedAt = n.UpdatedAt
      }).FirstOrDefaultAsync(n => n.Id == id);

    public async Task<List<NewsRecentsDto>?> GetByTitle(string title)
    {
      return await context.News
        .Include(n => n.Author)
        .Where(n => EF.Functions.ILike(n.Title, $"%{title}%"))
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
    }

    public async Task<NewsModel?> Create(NewsModel news)
    {
      context.News.Add(news);
      var saved = await context.SaveChangesAsync();
      return saved > 0 ? news : null;
    }

    public async Task<bool> Exists(string title) =>
      await context.News.AnyAsync(n => n.Title == title);

    public async Task<bool> Edit(NewsModel news)
    {
      if (!await Exists(news.Title)) return false;

      var findNews = await context.News.FirstOrDefaultAsync(n => n.Id == news.Id);
      if (findNews == null) return false;

      findNews.Title = news.Title;
      findNews.Description = news.Description;
      findNews.Content = news.Content;
      findNews.UpdatedAt = DateTime.UtcNow;

      context.News.Update(findNews);
      await context.SaveChangesAsync();

      return true;
    }

    public async Task<NewsModel?> Inactive(Guid id)
    {
      var news = await context.News.FirstOrDefaultAsync(n => n.Id == id);
      if (news is null) return null;

      news.Active = false;
      context.News.Update(news);

      var changes = await context.SaveChangesAsync();
      return changes > 0 ? news : null;
    }

    public async Task Clean()
    {
      var news = await context.News.ToListAsync();
      context.News.RemoveRange(news);
      await context.SaveChangesAsync();

    }
  }
}
