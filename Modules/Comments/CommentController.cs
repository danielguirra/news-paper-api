using Microsoft.AspNetCore.Mvc;
using Modules.Auth;
using Modules.Auth.UserContext;
using Modules.Comments.Dto;
using Modules.Comments.Model;
using Modules.Comments.Service;

namespace Modules.Comments.Controller;

[ApiController]
[Route("api/news/{newsId}/comments")]
public class CommentsController(CommentsService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(Guid newsId, int skip = 0, int take = 10)
    {
        var comments = await service.ListCommentsByNewsId(newsId, take, skip);
        return Ok(comments);
    }

    [HttpPost]
    [AuthRequired("user")]
    public async Task<IActionResult> Create(Guid newsId, [FromBody] CreateCommentDto dto)
    {
        var comment = await service.Create(
            new CommentModel
            {
                NewsId = newsId,
                Content = dto.Content,
                Likes = 0,
                DisLikes = 0,
                AuthorId = User.ToAuthModel().Id,
            }
        );

        return Created($"api/comments/{comment.Id}", comment);
    }

    [HttpPost("{id}/replies")]
    [AuthRequired("user")]
    public async Task<IActionResult> Reply(Guid newsId, Guid id, [FromBody] CreateCommentDto dto)
    {
        var comment = await service.Create(
            new CommentModel
            {
                NewsId = newsId,
                Content = dto.Content,
                ParentCommentId = id,
                Likes = 0,
                DisLikes = 0,
                AuthorId = User.ToAuthModel().Id,
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
        await service.Like(id, User.ToAuthModel().Id);
        return Ok();
    }

    [HttpPost("{id}/dislike")]
    [AuthRequired("user")]
    public async Task<IActionResult> DisLike(Guid id)
    {
        await service.DisLike(id, User.ToAuthModel().Id);
        return Ok();
    }
}
