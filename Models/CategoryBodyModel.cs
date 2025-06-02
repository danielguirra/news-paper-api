using System.ComponentModel.DataAnnotations;
using Utils;

namespace Models;

public class CategoryBodyModel
{
    [Required]
    [MaxLength(30)]
    [Unique]
    public required string Name { get; set; }
}
