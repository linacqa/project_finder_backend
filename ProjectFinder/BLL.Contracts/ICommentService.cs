using Base.BLL.Contracts;

namespace BLL.Contracts;

public interface ICommentService : IBaseService<BLL.DTO.Comment>
{
    Task<IEnumerable<BLL.DTO.Comment>> AllAsyncByProjectId(Guid projectId, Guid userId, bool isAdmin);
    
    public Task AddAsync(BLL.DTO.Comment entity, Guid userId, bool isAdmin);
}