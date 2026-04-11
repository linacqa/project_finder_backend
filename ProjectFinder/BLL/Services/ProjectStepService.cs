using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class ProjectStepService : BaseService<BLL.DTO.ProjectStep, DAL.DTO.ProjectStep, DAL.Contracts.IProjectStepRepository>, IProjectStepService
{
    private readonly IUserProjectRepository _userProjectRepository;
    
    public ProjectStepService(
        IAppUOW serviceUOW, 
        IMapper<ProjectStep, DAL.DTO.ProjectStep, Guid> mapper) : base(serviceUOW, serviceUOW.ProjectStepRepository, mapper)
    {
        _userProjectRepository = serviceUOW.UserProjectRepository;
    }
    
    public async Task<IEnumerable<BLL.DTO.ProjectStep>> AllAsyncByProjectId(Guid projectId, Guid userId)
    {
        var userInProject = await _userProjectRepository.UserInProject(projectId, userId);
        
        if (!userInProject)
        {
            throw new UnauthorizedAccessException("User is not part of the project.");
        }
        
        var entities = await ServiceRepository.AllAsyncByProjectId(projectId, userId);
        return entities.Select(e => Mapper.Map(e)!).ToList();
    }
    
    public override async Task<BLL.DTO.ProjectStep?> UpdateAsync(BLL.DTO.ProjectStep entity, Guid userId = default)
    {
        var userInProject = await _userProjectRepository.UserInProject(entity.ProjectId, userId);
        
        if (!userInProject)
        {
            throw new UnauthorizedAccessException("User is not part of the project.");
        }
        
        var dalEntity = Mapper.Map(entity);
        var updatedEntity = await ServiceRepository.UpdateAsync(dalEntity!, userId);
        return Mapper.Map(updatedEntity);
    }
}