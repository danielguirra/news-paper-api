using Microsoft.AspNetCore.Mvc;
using Models;
using Modules.Auth;

namespace Modules.Comments;

[ApiController]
[Route("api/news/{newsId}/comments")]
public class CommentController(CommentService service, AuthService authService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(Guid newsId, int skip = 0, int take = 10)
    {
        var comments = await service.ListCommentsByNewsId(newsId, take, skip);
        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid newsId, [FromBody] CreateCommentDto dto)
    {
        var auth = await authService.GetAuthUser();
        if (auth == null)
            return Unauthorized(new { message = "Token não fornecido e/ou inválido." });

        var comment = await service.Create(
            new CommentModel
            {
                NewsId = newsId,
                Content = dto.Content,
                Likes = 0,
                DisLikes = 0,
                AuthorId = auth.Id,
            }
        );

        return Created($"api/comments/{comment.Id}", comment);
    }

    [HttpDelete("{id}/inactive")]
    [AuthRequired("admin")]
    public async Task<IActionResult> Inactive(Guid id)
    {
        await service.Inactive(id);
        return Ok();
    }

    [HttpPost("{id}/like")]
    [AuthRequired("user")]
    public async Task<IActionResult> Like(Guid id)
    {
        var auth = await authService.GetAuthUser();
        if (auth == null)
            return Unauthorized(new { message = "Token não fornecido e/ou inválido." });
        await service.Like(id, auth.Id);
        return Ok();
    }

    [HttpPost("{id}/dislike")]
    [AuthRequired("user")]
    public async Task<IActionResult> DisLike(Guid id)
    {
        var auth = await authService.GetAuthUser();
        if (auth == null)
            return Unauthorized(new { message = "Token não fornecido e/ou inválido." });
        await service.DisLike(id, auth.Id);
        return Ok();
    }
}
