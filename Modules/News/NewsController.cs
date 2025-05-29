using Microsoft.AspNetCore.Mvc;

using Models;

using Services;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController(NewsService newsService, AuthService authService) : ControllerBase
{
  [HttpPost]
  public async Task<IActionResult> Create(NewsBodyModel newsBody)
  {
    var auth = await authService.GetAuthUser();
    if (auth == null)
      return Unauthorized(new { message = "Token não fornecido e/ou inválido." });

    if (await newsService.Exists(newsBody.Title)) return Conflict("Título de notícia já cadastrado.");

    var created = await newsService.Create(new NewsModel
    {
      Title = newsBody.Title,
      Description = newsBody.Description,
      Content = newsBody.Content,
      Thumbnail = newsBody.Thumbnail,
      AuthorId = auth.Id,
    });
    return created is null ? BadRequest() : Created("", new { created.Id });
  }

  [HttpPut]
  public async Task<IActionResult> Edit(NewsModel news)
  {

    if (await authService.GetAuthUser() == null)
      return Unauthorized(new { message = "Token não fornecido e/ou inválido." });

    var edited = await newsService.Edit(news);
    return edited is false ? BadRequest() : Ok(new { message = "Notícia atualizada." });
  }

  [HttpDelete("{id}/inactive")]
  public async Task<IActionResult> Inactivate(Guid id)
  {

    if (await authService.GetAuthUser() == null)
      return Unauthorized(new { message = "Token não fornecido e/ou inválido." });

    var result = await newsService.Inactive(id);
    return result is null ? NotFound() : Ok(new { message = "Notícia inativada." });
  }

  [HttpGet("recents")]
  public async Task<IActionResult> GetRecents([FromQuery] int take = 10, [FromQuery] int skip = 0)
  {
    var list = await newsService.Recents(take, skip);
    return list.Count != 0 ? Ok(list) : NotFound(new { message = "Nenhuma notícia encontrada." });
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
    var news = await newsService.GetByTitle(title);
    return news is null ? NotFound() : Ok(news);
  }


  [HttpDelete]
  public async Task<IActionResult> Delete()
  {
    await newsService.Clean();
    return Ok();
  }
}
