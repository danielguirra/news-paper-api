namespace Dto
{
  public class NewsDto
  {
    public required string Title { get; set; }
    public required string Subtitle { get; set; }
    public required string Content { get; set; }
    public required AuthorDto Author { get; set; }
    public Guid Id { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }

  public class NewsRecentsDto
  {
    public required string Title { get; set; }
    public required string Subtitle { get; set; }
    public Guid Id { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
  }

  public class AuthorDto
  {
    public required string Name { get; set; }
    public required string Role { get; set; }
    public Guid Id { get; set; }
  }

}