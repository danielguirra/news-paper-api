using System.ComponentModel.DataAnnotations;
using Modules.Category.Dto;
using Modules.User.Dto;
using Utils;

namespace Modules.News.Dto;

public class NewsBodyModelDto
{
    [Required]
    [MaxLength(100)]
    [Unique]
    public required string Title { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Description { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Thumbnail { get; set; }

    [Required]
    [MaxLength(2000)]
    public required string Content { get; set; }

    [Required]
    public required Guid CategoryId { get; set; }
}

public class NewsDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string? Content { get; set; }
    public required string Thumbnail { get; set; }

    public required CategoryDto Category { get; set; }
    public required AuthorDto Author { get; set; }
    public Guid Id { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class EditNewsDto
{
    public required Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Content { get; set; }
    public string? Thumbnail { get; set; }
    public bool? Active { get; set; }

    public Guid? CategoryId { get; set; }
}

public class NewsRecentsDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Thumbnail { get; set; }
    public Guid Id { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class NewsOnCategoryDto
{
    public required string Title { get; set; }
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
}
