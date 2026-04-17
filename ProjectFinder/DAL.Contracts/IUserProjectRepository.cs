using Base.DAL.Contracts;
using DAL.DTO;

namespace DAL.Contracts;

public interface IUserProjectRepository : IBaseRepository<UserProject>
{
    public Task<bool> UserInProject(Guid projectId, Guid userId);
    Task<IEnumerable<UserProject>> AllAsyncByProjectId(Guid projectId, Guid userId);
}