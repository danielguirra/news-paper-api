using Microsoft.AspNetCore.Mvc;

using Models;
using Models.AuthModel;

using Services;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController(NewsService newsService, UserService userService) : ControllerBase
{
  [HttpPost]
  public async Task<IActionResult> Create(NewsBodyModel newsBody)
  {
    var auth = await Auth();
    if (auth == null)
      return Unauthorized(new { message = "Token não fornecido e/ou inválido." });

    if (await newsService.Exists(newsBody.Title)) return Conflict("Título de notícia já cadastrado.");

    var created = await newsService.Create(new NewsModel
    {
      Title = newsBody.Title,
      Subtitle = newsBody.Subtitle,
      Content = newsBody.Content,
      AuthorId = auth.Id,
    });
    return created is null ? BadRequest() : Created("", new { created.Id });
  }

  [HttpPut]
  public async Task<IActionResult> Edit(NewsModel news)
  {
    var auth = await Auth();
    if (auth == null)
      return Unauthorized(new { message = "Token não fornecido e/ou inválido." });

    var edited = await newsService.Edit(news);
    return edited is false ? BadRequest() : Ok(new { message = "Notícia atualizada." });
  }

  [HttpDelete("{id:guid}/inactive")]
  public async Task<IActionResult> Inactivate(Guid id)
  {
    var auth = await Auth();
    if (auth == null)
      return Unauthorized(new { message = "Token não fornecido e/ou inválido." });

    var result = await newsService.Inactive(id);
    return result is null ? NotFound() : Ok(new { message = "Notícia inativada." });
  }

  [HttpGet("recents")]
  public async Task<IActionResult> GetRecents([FromQuery] int take = 10, [FromQuery] int skip = 0)
  {
    var list = await newsService.Recents(take, skip);
    return list.Any() ? Ok(list) : NotFound(new { message = "Nenhuma notícia encontrada." });
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetById(Guid id)
  {
    var news = await newsService.GetOneById(id);
    return news is null ? NotFound() : Ok(news);
  }

  [HttpGet("title/{title}")]
  public async Task<IActionResult> GetByTitle(string title)
  {
    var news = await newsService.GetOneByTitle(title);
    return news is null ? NotFound() : Ok(news);
  }

  private async Task<AuthModel?> Auth()
  {
    var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
    if (authHeader == null || !authHeader.StartsWith("Bearer ")) return null;

    var token = authHeader.Substring("Bearer ".Length).Trim();
    return await userService.Me(token);
  }

  [HttpDelete]
  public async Task<IActionResult> Delete()
  {
    await newsService.Clean();
    return Ok();
  }
}
