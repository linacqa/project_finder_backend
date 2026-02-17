using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;
using ProjectFolder = DAL.DTO.ProjectFolder;

namespace BLL.Services;

public class ProjectService : BaseService<BLL.DTO.Project, DAL.DTO.Project, DAL.Contracts.IProjectRepository>, IProjectService
{
    private readonly IProjectFolderRepository _projectFolderRepository;
    private readonly IProjectTagRepository _projectTagRepository;
    private readonly IProjectStepRepository _projectStepRepository;
    private readonly IUserProjectRepository _userProjectRepository;

    public ProjectService(
        IAppUOW serviceUOW, 
        IMapper<Project, DAL.DTO.Project, Guid> mapper) : base(serviceUOW, serviceUOW.ProjectRepository, mapper)
    {
        _projectFolderRepository = serviceUOW.ProjectFolderRepository;
        _projectTagRepository = serviceUOW.ProjectTagRepository;
        _projectStepRepository = serviceUOW.ProjectStepRepository;
        _userProjectRepository = serviceUOW.UserProjectRepository;
    }
    
    public override void Add(BLL.DTO.Project entity, Guid userId = default)
    {
        var dalEntity = Mapper.Map(entity);
        ServiceRepository.Add(dalEntity!, userId);
        
        // TODO: check for existence of entities with such ids before adding
        
        if (entity.FolderIds.Count > 0)
        {
            foreach (var folderId in entity.FolderIds)
            {
                _projectFolderRepository.Add(new ProjectFolder()
                {
                    FolderId = folderId,
                    ProjectId = dalEntity!.Id
                });
            }
        }
        
        if (entity.TagIds.Count > 0) 
        {
            foreach (var tagId in entity.TagIds)
            {
                _projectTagRepository.Add(new DAL.DTO.ProjectTag()
                {
                    TagId = tagId,
                    ProjectId = dalEntity!.Id
                });
            }
        }
        
        if (entity.StepIds.Count > 0)
        {
            foreach (var stepId in entity.StepIds)
            {
                _projectStepRepository.Add(new DAL.DTO.ProjectStep()
                {
                    StepId = stepId,
                    ProjectId = dalEntity!.Id,
                    StepStatusId = Guid.Parse("00000000-0000-0000-0000-000000000001"), // not started
                });
            }
        }

        _userProjectRepository.Add(new DAL.DTO.UserProject()
        {
            ProjectId = dalEntity!.Id,
            UserId = entity.AuthorId,
            UserProjectRoleId = Guid.Parse("00000000-0000-0000-0000-000000000001"), // author
        });
        
        if (entity.PrimarySupervisorId != null)
        {
            _userProjectRepository.Add(new DAL.DTO.UserProject()
            {
                ProjectId = dalEntity!.Id,
                UserId = entity.PrimarySupervisorId.Value,
                UserProjectRoleId = Guid.Parse("00000000-0000-0000-0000-000000000002"), // primary supervisor
            });
        }
        
        if (entity.ExternalSupervisorId != null)
        {
            _userProjectRepository.Add(new DAL.DTO.UserProject()
            {
                ProjectId = dalEntity!.Id,
                UserId = entity.ExternalSupervisorId.Value,
                UserProjectRoleId = Guid.Parse("00000000-0000-0000-0000-000000000003"), // external supervisor
            });
        }
    }
}