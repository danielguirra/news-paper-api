using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Models;
using Modules.News;
using Modules.User;

namespace Modules.Comments
{
    public class CommentModel : BaseModel
    {
        [Required]
        public required Guid AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public UserModel? Author { get; set; }

        [Required]
        public required Guid NewsId { get; set; }

        [ForeignKey(nameof(NewsId))]
        public NewsModel? News { get; set; }

        [Required]
        [MaxLength(500)]
        [MinLength(3)]
        public required string Content { get; set; }

        [Required]
        public required int Likes { get; set; } = 0;

        [Required]
        public required int DisLikes { get; set; } = 0;

        [AllowNull]
        public Guid? ParentCommentId { get; set; }

        [ForeignKey(nameof(ParentCommentId))]
        public CommentModel? ParentComment { get; set; } // Navegação para o comentário pai

        [AllowNull]
        public ICollection<CommentModel>? Replies { get; set; }
    }
}
