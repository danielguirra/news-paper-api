using System.ComponentModel.DataAnnotations;
using Models;
using Utils;

namespace Modules.Category
{
    public class CategoryModel : BaseModel
    {
        [Required(ErrorMessage = "Name é obrigatório")]
        [MinLength(6)]
        [Unique]
        [MaxLength(50)]
        public required string Name { get; set; }
    }
}
