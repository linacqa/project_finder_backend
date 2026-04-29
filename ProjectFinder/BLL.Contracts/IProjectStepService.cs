using Base.BLL.Contracts;

namespace BLL.Contracts;

public interface IProjectStepService : IBaseService<BLL.DTO.ProjectStep>
{
    Task<IEnumerable<BLL.DTO.ProjectStep>> AllAsyncByProjectId(Guid projectId, Guid userId, bool isAdmin);
    Task<BLL.DTO.ProjectStep?> UpdateAsync(BLL.DTO.ProjectStep entity, Guid userId, bool isAdmin);
}