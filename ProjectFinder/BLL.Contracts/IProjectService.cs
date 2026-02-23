using Base.BLL.Contracts;
using Base.DTO;
using DTO.v1;

namespace BLL.Contracts;

public interface IProjectService : IBaseService<BLL.DTO.Project>
{
    Task<PageResult<BLL.DTO.Project>> SearchAsync(ProjectsSearchRequest request, Guid userId = default);
}