using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models;
using Modules.User.Model;

namespace Modules.Comments.Model;

public class CommentsReactionModel : BaseModel
{
    [Required]
    public required Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public UserModel? User { get; set; }

    [Required]
    public required Guid CommentId { get; set; }

    [ForeignKey(nameof(CommentId))]
    public CommentModel? Comment { get; set; }

    [Required]
    public required bool IsLike { get; set; }
}
