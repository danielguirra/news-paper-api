namespace Modules.User.Dto;

public class AuthorDto
{
    public required string Name { get; set; }
    public required string Role { get; set; }
    public Guid Id { get; set; }
}

public class EditUserDto
{
    public string? Name { get; set; }
    public string? Role { get; set; }
    public string? Email { get; set; }
    public required Guid Id { get; set; }
    public required string Password { get; set; }
    public string? NewPassword { get; set; }
}

public class LoginBodyModelDto
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}

public class LoginResponse
{
    public required string Token { get; set; }
}
