using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class ProjectFolderService : BaseService<BLL.DTO.ProjectFolder, DAL.DTO.ProjectFolder, DAL.Contracts.IProjectFolderRepository>, IProjectFolderService
{
    public ProjectFolderService(
        IAppUOW serviceUOW, 
        IMapper<ProjectFolder, DAL.DTO.ProjectFolder, Guid> mapper) : base(serviceUOW, serviceUOW.ProjectFolderRepository, mapper)
    {
    }
}