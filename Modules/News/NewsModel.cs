using System.ComponentModel.DataAnnotations;
using Utils;

namespace Models
{
  public class NewsModel : BaseModel
  {
    [Required]
    [MaxLength(100)]
    [Unique]
    public required string Title { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Subtitle { get; set; }

    [Required]
    [MaxLength(2000)]
    public required string Content { get; set; }

    public Guid AuthorId { get; set; }
    public required UserModel Author { get; set; }
  }
}
