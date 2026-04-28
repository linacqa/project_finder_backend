using Base.BLL.Contracts;

namespace BLL.Contracts;

public interface IProjectStepService : IBaseService<BLL.DTO.ProjectStep>
{
    Task<IEnumerable<BLL.DTO.ProjectStep>> AllAsyncByProjectId(Guid projectId, Guid userId, bool isAdmin);
}