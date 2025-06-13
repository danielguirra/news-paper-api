namespace Modules.Auth.Model;

public class AuthModel
{
    public required string Name { get; set; }

    public required string Role { get; set; }

    public required Guid Id { get; set; }
}
