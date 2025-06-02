using System.ComponentModel.DataAnnotations;
using Utils;

namespace Models
{
    public class NewsBodyModel
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
}
