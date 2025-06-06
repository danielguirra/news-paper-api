using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Modules.Auth;

namespace Modules.User;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserService service) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create(UserModel newUser)
    {
        var auth = User.ToAuthModel();
        if (auth == null)
        {
            newUser.Role = "user";
        }
        var created = await service.Create(newUser, auth);
        return Created("", new { created.Id });
    }

    [HttpPut]
    [AuthRequired]
    public async Task<IActionResult> Edit(EditUserDto user)
    {
        await service.Edit(user, User.ToAuthModel());
        return Ok(new { message = "Usuário editado." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel login)
    {
        var token = await service.GetTokenAsync(login);
        return StatusCode(201, new { token, message = "Login sucesso" });
    }

    [HttpDelete("{id}/inactive")]
    [AuthRequired]
    public async Task<IActionResult> Inactive(Guid id)
    {
        await service.Inactivate(id);
        return Ok(new { message = "Usuário inativado." });
    }

    [HttpGet("me")]
    [AuthRequired]
    public IActionResult Me()
    {
        return Ok(User.ToAuthModel());
    }
}
