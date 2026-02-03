using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class ProjectTypeService : BaseService<BLL.DTO.ProjectType, DAL.DTO.ProjectType, DAL.Contracts.IProjectTypeRepository>, IProjectTypeService
{
    public ProjectTypeService(
        IAppUOW serviceUOW, 
        IMapper<ProjectType, DAL.DTO.ProjectType, Guid> mapper) : base(serviceUOW, serviceUOW.ProjectTypeRepository, mapper)
    {
    }
}