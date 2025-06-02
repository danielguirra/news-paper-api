using Microsoft.AspNetCore.Mvc;
using Models;
using Modules.Auth;

namespace Modules.News;

[ApiController]
[Route("api/[controller]")]
public class NewsController(NewsService newsService, AuthService authService) : ControllerBase
{
    [HttpPost]
    [AuthRequired("author")]
    public async Task<IActionResult> Create(NewsBodyModel newsBody)
    {
        var auth = await authService.GetAuthUser();
        if (auth == null)
            return Unauthorized(new { message = "Token não fornecido e/ou inválido." });

        var created = await newsService.Create(
            new NewsModel
            {
                Title = newsBody.Title,
                Description = newsBody.Description,
                Content = newsBody.Content,
                Thumbnail = newsBody.Thumbnail,
                CategoryId = newsBody.CategoryId,
                AuthorId = auth.Id,
            }
        );

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { created.Id });
    }

    [HttpPut]
    [AuthRequired("admin")]
    public async Task<IActionResult> Edit(EditNewsDto news)
    {
        await newsService.Edit(news);
        return Ok(new { message = "Notícia atualizada." });
    }

    [HttpDelete("{id}/inactive")]
    [AuthRequired("admin")]
    public async Task<IActionResult> Inactivate(Guid id)
    {
        await newsService.Inactive(id);
        return Ok(new { message = "Notícia inativada." });
    }

    [HttpGet("recents")]
    public async Task<IActionResult> GetRecents([FromQuery] int take = 10, [FromQuery] int skip = 0)
    {
        return Ok(await newsService.Recents(take, skip));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await newsService.GetOneById(id));
    }

    [HttpGet("title/{title}")]
    public async Task<IActionResult> GetByTitle(string title)
    {
        return Ok(await newsService.GetByTitle(title));
    }

    // [HttpDelete]
    // public async Task<IActionResult> Delete()
    // {
    //     await newsService.Clean();
    //     return Ok();
    // }
}
