using Base.DAL.Contracts;
using DAL.DTO;

namespace DAL.Contracts;

public interface IProjectFolderRepository : IBaseRepository<ProjectFolder>
{
    Task<IEnumerable<ProjectFolder>> AllAsyncByProjectId(Guid projectId, Guid userId);
}