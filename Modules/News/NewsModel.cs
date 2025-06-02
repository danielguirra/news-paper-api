using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models;
using Modules.Category;
using Modules.User;
using Utils;

namespace Modules.News
{
    public class NewsModel : BaseModel
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
        public required Guid AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public UserModel? Author { get; set; }

        [Required]
        public required Guid CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public CategoryModel? Category { get; set; }
    }
}
