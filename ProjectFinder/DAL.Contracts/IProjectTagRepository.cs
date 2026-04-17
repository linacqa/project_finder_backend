using Base.DAL.Contracts;
using DAL.DTO;

namespace DAL.Contracts;

public interface IProjectTagRepository : IBaseRepository<ProjectTag>
{
    Task<IEnumerable<ProjectTag>> AllAsyncByProjectId(Guid projectId, Guid userId);
}