namespace Modules.Comments.Dto;

public class CreateCommentDto
{
    public required string Content { get; set; }
}

public class CommentNewsDto
{
    public required Guid Id { get; set; }
    public required string Content { get; set; }
    public required string AuthorName { get; set; }
    public ICollection<CommentNewsDto>? Replies { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Likes { get; set; }
    public int DisLikes { get; set; }
}
