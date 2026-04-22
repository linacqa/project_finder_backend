using Base.DAL.Contracts;
using DAL.DTO;

namespace DAL.Contracts;

public interface ICommentRepository : IBaseRepository<Comment>
{
    Task<IEnumerable<DAL.DTO.Comment>> AllAsyncByProjectId(Guid projectId, Guid userId);
    Task<bool> CommentHasReplies(Guid commentId);
}