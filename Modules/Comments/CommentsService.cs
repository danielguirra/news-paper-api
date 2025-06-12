using Data;
using Exceptions;
using Microsoft.EntityFrameworkCore;
using Modules.Comments.Dto;
using Modules.Comments.Model;

namespace Modules.Comments.Service
{
    public class CommentsService(AppDbContext context) : BaseService(context)
    {
        public async Task<CommentModel> Create(CommentModel commet)
        {
            if (!await AliveNews(commet.NewsId))
                throw new NewsNotFoundException(commet.NewsId);

            if (commet.ParentCommentId != null)
                if (!await GetCommentToReply((Guid)commet.ParentCommentId))
                    throw new CommentNotFoundException(commet.ParentCommentId);

            context.Comments.Add(commet);
            await SaveAsync();

            return commet;
        }

        private async Task<bool> AliveNews(Guid id) => await context.News.AnyAsync(n => n.Id == id);

        private async Task<bool> GetCommentToReply(Guid id) =>
            await context.Comments.AnyAsync(n =>
                n.Id == id && n.Active && n.ParentCommentId == null
            );

        public async Task<List<CommentNewsDto>> ListCommentsByNewsId(Guid id, int take, int skip)
        {
            if (take <= 0 || skip < 0)
                throw new BadRequestTakeSkip();

            List<CommentNewsDto>? comments = await context
                .Comments.Include(c => c.Author)
                .Include(c => c.Replies)
                .Where(c => c.NewsId == id && c.Active && c.ParentCommentId == null)
                .OrderByDescending(c => c.Likes)
                .Skip(skip)
                .Take(take)
                .Select(c => new CommentNewsDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    AuthorName = c.Author!.Name,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    Likes = c.Likes,
                    DisLikes = c.DisLikes,
                    Replies = (
                        c.Replies != null && c.Replies.Any()
                            ? c
                                .Replies.Where(r => r.Active)
                                .OrderBy(r => r.CreatedAt)
                                .Select(r => new CommentNewsDto
                                {
                                    Id = r.Id,
                                    Content = r.Content,
                                    AuthorName = r.Author!.Name,
                                    CreatedAt = r.CreatedAt,
                                    UpdatedAt = r.UpdatedAt,
                                    Likes = r.Likes,
                                    DisLikes = r.DisLikes,
                                })
                                .ToList()
                            : new List<CommentNewsDto>()
                    ),
                })
                .ToListAsync();

            if (comments.Count == 0)
                throw new CommentNotFoundException(null);

            return comments;
        }

        public async Task Inactive(Guid id)
        {
            CommentModel findComment =
                await context.Comments.FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new CommentNotFoundException(id);
            findComment.Active = false;
            findComment.UpdatedAt = DateTime.UtcNow;
            context.Comments.Update(findComment);
            await SaveAsync();
        }

        public async Task Like(Guid commentId, Guid userId)
        {
            CommentsReactionModel? reaction = await context.CommentReactions.FirstOrDefaultAsync(
                r => r.CommentId == commentId && r.UserId == userId
            );

            if (reaction != null)
            {
                if (reaction.IsLike)
                    throw new AlreadyLikedException();

                reaction.IsLike = true;
                await AdjustReactionCounts(commentId, +1, -1);
            }
            else
            {
                context.CommentReactions.Add(
                    new CommentsReactionModel
                    {
                        CommentId = commentId,
                        UserId = userId,
                        IsLike = true,
                    }
                );

                await AdjustReactionCounts(commentId, +1, 0);
            }

            await context.SaveChangesAsync();
        }

        public async Task DisLike(Guid commentId, Guid userId)
        {
            CommentsReactionModel? reaction = await context.CommentReactions.FirstOrDefaultAsync(
                r => r.CommentId == commentId && r.UserId == userId
            );

            if (reaction != null)
            {
                if (!reaction.IsLike)
                    throw new AlreadyLikedException();

                reaction.IsLike = false;
                await AdjustReactionCounts(commentId, -1, +1);
            }
            else
            {
                context.CommentReactions.Add(
                    new CommentsReactionModel
                    {
                        CommentId = commentId,
                        UserId = userId,
                        IsLike = false,
                    }
                );

                await AdjustReactionCounts(commentId, 0, +1);
            }

            await context.SaveChangesAsync();
        }

        private async Task AdjustReactionCounts(Guid commentId, int likeDelta, int dislikeDelta)
        {
            CommentModel comment =
                await context.Comments.FirstAsync(c => c.Id == commentId)
                ?? throw new CommentNotFoundException(commentId);
            comment.Likes += likeDelta;
            comment.DisLikes += dislikeDelta;
        }
    }
}
