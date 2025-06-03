namespace Models
{
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

    public class CategoryDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }

    public class AuthorDto
    {
        public required string Name { get; set; }
        public required string Role { get; set; }
        public Guid Id { get; set; }
    }

    public class EditUserDto
    {
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? Email { get; set; }
        public required Guid Id { get; set; }
        public required string Password { get; set; }
        public string? NewPassword { get; set; }
    }

    public class CreateCommentDto
    {
        public required string Content { get; set; }
    }

    public class CommentNewsDto
    {
        public required Guid Id { get; set; }
        public required string Content { get; set; }
        public required string AuthorName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int Likes { get; set; }
        public int DisLikes { get; set; }
    }
}
