using Microsoft.AspNetCore.Mvc;
using Models;
using Modules.Auth;

namespace Modules.User;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserService service, AuthService authService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(UserModel user)
    {
        if (await service.Exists(user))
            return Conflict("Verifique o email ou nome do usuário");

        var created = await service.Create(user);
        return created is null ? BadRequest() : Created("", new { created.Id });
    }

    [HttpPut]
    [AuthRequired]
    public async Task<IActionResult> Edit(EditUserDto user)
    {
        var edited = await service.Edit(user);
        return edited is false ? BadRequest() : Created();
    }

    // [HttpGet]
    // public async Task<IActionResult> ListUsers()
    // {
    //     var users = await service.ListAll();
    //     return users.Any() ? Ok(users) : NotFound();
    // }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel login)
    {
        var token = await service.GetTokenAsync(login);

        if (token == null)
            return Unauthorized("Email ou senha incorretos");

        string message = "Login sucesso";
        return StatusCode(201, new { token, message });
    }

    // [HttpDelete]
    // public async Task<IActionResult> Delete()
    // {
    //     await service.Clean();
    //     return Ok();
    // }

    [HttpDelete("{id}/inactive")]
    [AuthRequired]
    public async Task<IActionResult> Inactive(Guid id)
    {
        var result = await service.Inactive(id);
        return result is null ? NotFound() : Ok(new { message = "Usuario inativado." });
    }

    [HttpGet("me")]
    [AuthRequired]
    public async Task<IActionResult> Me()
    {
        var auth = await authService.GetAuthUser();
        if (auth == null)
            return Unauthorized(new { message = "Token não fornecido e/ou inválido." });

        return Ok(auth);
    }
}
