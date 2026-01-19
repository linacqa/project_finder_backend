using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class ProjectService : BaseService<BLL.DTO.Project, DAL.DTO.Project, DAL.Contracts.IProjectRepository>, IProjectService
{
    public ProjectService(
        IAppUOW serviceUOW, 
        IMapper<Project, DAL.DTO.Project, Guid> mapper) : base(serviceUOW, serviceUOW.ProjectRepository, mapper)
    {
    }
}