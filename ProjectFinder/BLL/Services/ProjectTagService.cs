using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class ProjectTagService : BaseService<BLL.DTO.ProjectTag, DAL.DTO.ProjectTag, DAL.Contracts.IProjectTagRepository>, IProjectTagService
{
    public ProjectTagService(
        IAppUOW serviceUOW, 
        IMapper<ProjectTag, DAL.DTO.ProjectTag, Guid> mapper) : base(serviceUOW, serviceUOW.ProjectTagRepository, mapper)
    {
    }
}