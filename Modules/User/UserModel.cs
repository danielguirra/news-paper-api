using System.ComponentModel.DataAnnotations;

using Utils;

namespace Models
{
  public class UserModel : BaseModel
  {
    [Required(ErrorMessage = "Name é obrigatório")]
    [MaxLength(255)]
    [Unique]
    public required string Name { get; set; }


    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [MaxLength(100)]
    [Unique]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password é obrigatório")]
    [MinLength(8)]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Role é obrigatório")]
    [MaxLength(6)]
    public string Role { get; set; } = "user";

    public ICollection<NewsModel> News { get; set; } = new List<NewsModel>();
  }
}
