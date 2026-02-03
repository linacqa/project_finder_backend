using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class ProjectStatusService : BaseService<BLL.DTO.ProjectStatus, DAL.DTO.ProjectStatus, DAL.Contracts.IProjectStatusRepository>, IProjectStatusService
{
    public ProjectStatusService(
        IAppUOW serviceUOW, 
        IMapper<ProjectStatus, DAL.DTO.ProjectStatus, Guid> mapper) : base(serviceUOW, serviceUOW.ProjectStatusRepository, mapper)
    {
    }
}