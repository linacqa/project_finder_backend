using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class ProjectStepService : BaseService<BLL.DTO.ProjectStep, DAL.DTO.ProjectStep, DAL.Contracts.IProjectStepRepository>, IProjectStepService
{
    public ProjectStepService(
        IAppUOW serviceUOW, 
        IMapper<ProjectStep, DAL.DTO.ProjectStep, Guid> mapper) : base(serviceUOW, serviceUOW.ProjectStepRepository, mapper)
    {
    }
}