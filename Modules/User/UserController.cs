using Microsoft.AspNetCore.Mvc;

using Models;
using Models.AuthModel;

using Services;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController(UserService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(UserModel user)
    {
        if (await service.Exists(user)) return Conflict("Verifique o email ou nome do usuário");

        var created = await service.Create(user);
        return created is null ? BadRequest() : Created("", new { created.Id });
    }

    [HttpPut]
    public async Task<IActionResult> Edit(UserModel user)

    {
        var auth = await Auth();
        if (auth == null)
            return Unauthorized(new { message = "Token não fornecido e/ou inválido." });

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
        var token = await service.Login(login);

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

    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var auth = await Auth();
        if (auth == null)
            return Unauthorized(new { message = "Token não fornecido e/ou inválido." });

        return Ok(auth);
    }

    private async Task<AuthModel?> Auth()
    {
        var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

        if (authHeader == null || !authHeader.StartsWith("Bearer ")) return null;

        var token = authHeader.Substring("Bearer ".Length).Trim();

        var auth = await service.Me(token);

        return auth;
    }

}
