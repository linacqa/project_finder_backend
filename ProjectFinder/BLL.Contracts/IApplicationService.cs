using Base.BLL.Contracts;

namespace BLL.Contracts;

public interface IApplicationService : IBaseService<BLL.DTO.Application>
{
    Task<BLL.DTO.Application?> FindAsyncByProjectId(Guid projectId, Guid userId);
    Task AddWithValidationAsync(BLL.DTO.Application entity, Guid userId = default);
}
