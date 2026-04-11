using Base.DAL.Contracts;
using DAL.DTO;

namespace DAL.Contracts;

public interface IProjectStepRepository : IBaseRepository<ProjectStep>
{
    Task<IEnumerable<DAL.DTO.ProjectStep>> AllAsyncByProjectId(Guid projectId, Guid userId);
}