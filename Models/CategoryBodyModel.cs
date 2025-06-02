using System.ComponentModel.DataAnnotations;
using Utils;

public class CategoryBodyModel
{
    [Required]
    [MaxLength(30)]
    [Unique]
    public required string Name { get; set; }
}
