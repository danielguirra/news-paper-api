using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Modules.Auth;
using Modules.Auth.UserContext;
using Modules.News.Dto;
using Modules.News.Model;
using Modules.News.Service;

namespace Modules.News.Controller;

[ApiController]
[Route("api/news")]
[EnableRateLimiting("fixed")]
public class NewsController(NewsService newsService) : ControllerBase
{
    [HttpPost]
    [AuthRequired("author")]
    [EnableRateLimiting("authenticated")]
    public async Task<IActionResult> Create(NewsBodyModelDto newsBody)
    {
        NewsModel created = await newsService.Create(
            new NewsModel
            {
                Title = newsBody.Title,
                Description = newsBody.Description,
                Content = newsBody.Content,
                Thumbnail = newsBody.Thumbnail,
                CategoryId = newsBody.CategoryId,
                AuthorId = User.ToAuthModel().Id,
            }
        );

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { created.Id });
    }

    [HttpPut]
    [AuthRequired("admin")]
    [EnableRateLimiting("authenticated")]
    public async Task<IActionResult> Edit(EditNewsDto news)
    {
        await newsService.Edit(news);
        return Ok(new { message = "Notícia atualizada." });
    }

    [HttpDelete("{id}/inactive")]
    [AuthRequired("admin")]
    [EnableRateLimiting("authenticated")]
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
