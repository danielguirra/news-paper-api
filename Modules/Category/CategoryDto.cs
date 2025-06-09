using System.ComponentModel.DataAnnotations;
using Utils;

namespace Modules.Category.Dto;

public class CategoryDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}

public class CategoryBodyModelDto
{
    [Required]
    [MaxLength(30)]
    [Unique]
    public required string Name { get; set; }
}
