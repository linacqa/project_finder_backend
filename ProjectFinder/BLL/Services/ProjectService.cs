using Base.BLL;
using Base.Contracts;
using Base.DTO;
using BLL.Contracts;
using DAL.Contracts;
using DTO.v1;
using Project = BLL.DTO.Project;
using ProjectFolder = DAL.DTO.ProjectFolder;

namespace BLL.Services;

public class ProjectService : BaseService<BLL.DTO.Project, DAL.DTO.Project, DAL.Contracts.IProjectRepository>, IProjectService
{
    private static readonly Guid NotStartedStepStatusId = new("00000000-0000-0000-0000-000000000001");
    private static readonly Guid AuthorRoleId = new("00000000-0000-0000-0000-000000000001");
    private static readonly Guid PrimarySupervisorRoleId = new("00000000-0000-0000-0000-000000000002");
    private static readonly Guid ExternalSupervisorRoleId = new("00000000-0000-0000-0000-000000000003");

    private readonly IProjectFolderRepository _projectFolderRepository;
    private readonly IProjectTagRepository _projectTagRepository;
    private readonly IProjectStepRepository _projectStepRepository;
    private readonly IUserProjectRepository _userProjectRepository;
    private readonly IFolderRepository _folderRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IStepRepository _stepRepository;

    public ProjectService(
        IAppUOW serviceUOW, 
        IMapper<Project, DAL.DTO.Project, Guid> mapper) : base(serviceUOW, serviceUOW.ProjectRepository, mapper)
    {
        _projectFolderRepository = serviceUOW.ProjectFolderRepository;
        _projectTagRepository = serviceUOW.ProjectTagRepository;
        _projectStepRepository = serviceUOW.ProjectStepRepository;
        _userProjectRepository = serviceUOW.UserProjectRepository;
        _folderRepository = serviceUOW.FolderRepository;
        _tagRepository = serviceUOW.TagRepository;
        _stepRepository = serviceUOW.StepRepository;
    }
    
    public async Task<IEnumerable<BLL.DTO.Project>> AllCurrentUserAsync(Guid userId = default)
    {
        var entities = await ServiceRepository.AllCurrentUserAsync(userId);
        return entities.Select(e => Mapper.Map(e)!).ToList(); 
    }
    
    public async Task<PageResult<BLL.DTO.Project>> SearchAsync(ProjectsSearchRequest request, Guid userId = default)
    {
        request.Page = request.Page <= 0 ? 1 : request.Page;
        request.PageSize = request.PageSize > 100 ? 100 : request.PageSize;

        var (entities, totalCount) =
            await ServiceRepository.SearchAsync(request, userId);

        return new PageResult<BLL.DTO.Project>
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            Data = entities.Select(e => Mapper.Map(e)!).ToList()
        };
    }
    
    public override void Add(BLL.DTO.Project entity, Guid userId = default)
    {
        var dalEntity = Mapper.Map(entity);
        ServiceRepository.Add(dalEntity!, userId);
        
        // Check for existence of entities with such ids before adding
        
        if (entity.FolderIds.Count > 0)
        {
            foreach (var folderId in entity.FolderIds)
            {
                if (!_folderRepository.Exists(folderId))
                {
                    throw new ArgumentException($"Folder with id {folderId} does not exist.");
                }
            }
        }
        
        if (entity.TagIds.Count > 0) 
        {
            foreach (var tagId in entity.TagIds)
            {
                if (!_tagRepository.Exists(tagId))
                {
                    throw new ArgumentException($"Tag with id {tagId} does not exist.");
                }
            }
        }
        
        if (entity.StepIds.Count > 0)
        {
            foreach (var stepId in entity.StepIds)
            {
                if (!_stepRepository.Exists(stepId))
                {
                    throw new ArgumentException($"Step with id {stepId} does not exist.");
                }
            }
        }
        
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
            foreach (var stepId in entity.StepIds.Select((value, i) => new { value, i }))
            {
                _projectStepRepository.Add(new DAL.DTO.ProjectStep()
                {
                    StepId = stepId.value,
                    ProjectId = dalEntity!.Id,
                    StepStatusId = NotStartedStepStatusId, // not started
                    Order = stepId.i
                });
            }
        }

        _userProjectRepository.Add(new DAL.DTO.UserProject()
        {
            ProjectId = dalEntity!.Id,
            UserId = entity.AuthorId,
            UserProjectRoleId = AuthorRoleId, // author
        });
        
        if (entity.PrimarySupervisorId != null)
        {
            _userProjectRepository.Add(new DAL.DTO.UserProject()
            {
                ProjectId = dalEntity!.Id,
                UserId = entity.PrimarySupervisorId.Value,
                UserProjectRoleId = PrimarySupervisorRoleId, // primary supervisor
            });
        }
        
        if (entity.ExternalSupervisorId != null)
        {
            _userProjectRepository.Add(new DAL.DTO.UserProject()
            {
                ProjectId = dalEntity!.Id,
                UserId = entity.ExternalSupervisorId.Value,
                UserProjectRoleId = ExternalSupervisorRoleId, // external supervisor
            });
        }
    }
    
    public override async Task<BLL.DTO.Project?> UpdateAsync(BLL.DTO.Project entity, Guid userId = default)
    {
        var dalEntity = Mapper.Map(entity);
        var updatedEntity = await ServiceRepository.UpdateAsync(dalEntity!, userId);
        if (updatedEntity == null)
        {
            return null;
        }

        var newFolderIds = (entity.FolderIds ?? new List<Guid>()).Distinct().ToHashSet();
        var newTagIds = (entity.TagIds ?? new List<Guid>()).Distinct().ToHashSet();
        var newStepIds = (entity.StepIds ?? new List<Guid>()).Distinct().ToHashSet();
        
        foreach (var folderId in newFolderIds)
        {
            if (!_folderRepository.Exists(folderId))
            {
                throw new ArgumentException($"Folder with id {folderId} does not exist.");
            }
        }
        
        foreach (var tagId in newTagIds)
        {
            if (!_tagRepository.Exists(tagId))
            {
                throw new ArgumentException($"Tag with id {tagId} does not exist.");
            }
        }
        
        foreach (var stepId in newStepIds)
        {
            if (!_stepRepository.Exists(stepId))
            {
                throw new ArgumentException($"Step with id {stepId} does not exist.");
            }
        }

        var existingProjectFolders = (await _projectFolderRepository.AllAsyncByProjectId(dalEntity!.Id, userId)).ToList();
        foreach (var projectFolder in existingProjectFolders.Where(e => !newFolderIds.Contains(e.FolderId)))
        {
            _projectFolderRepository.Remove(projectFolder.Id);
        }

        var existingFolderIds = existingProjectFolders.Select(e => e.FolderId).ToHashSet();
        foreach (var folderId in newFolderIds.Where(id => !existingFolderIds.Contains(id)))
        {
            _projectFolderRepository.Add(new ProjectFolder()
            {
                FolderId = folderId,
                ProjectId = dalEntity.Id
            });
        }

        var existingProjectTags = (await _projectTagRepository.AllAsyncByProjectId(dalEntity.Id, userId)).ToList();
        foreach (var projectTag in existingProjectTags.Where(e => !newTagIds.Contains(e.TagId)))
        {
            _projectTagRepository.Remove(projectTag.Id);
        }

        var existingTagIds = existingProjectTags.Select(e => e.TagId).ToHashSet();
        foreach (var tagId in newTagIds.Where(id => !existingTagIds.Contains(id)))
        {
            _projectTagRepository.Add(new DAL.DTO.ProjectTag()
            {
                TagId = tagId,
                ProjectId = dalEntity.Id
            });
        }

        var existingProjectSteps = (await _projectStepRepository.AllAsyncByProjectId(dalEntity.Id, userId)).ToList();
        foreach (var projectStep in existingProjectSteps.Where(e => !newStepIds.Contains(e.StepId)))
        {
            _projectStepRepository.Remove(projectStep.Id);
        }

        var existingStepIds = existingProjectSteps.Select(e => e.StepId).ToHashSet();
        foreach (var stepId in newStepIds.Select((value, i) => new { value, i }))
        {
            if (existingStepIds.Contains(stepId.value))
            {
                continue;
            }
            _projectStepRepository.Add(new DAL.DTO.ProjectStep()
            {
                StepId = stepId.value,
                ProjectId = dalEntity.Id,
                StepStatusId = NotStartedStepStatusId, // not started
                Order = stepId.i
            });
        }

        var trackedRoleIds = new HashSet<Guid>
        {
            AuthorRoleId,
            PrimarySupervisorRoleId,
            ExternalSupervisorRoleId
        };

        var targetAssignments = new HashSet<(Guid UserId, Guid RoleId)>
        {
            (entity.AuthorId, AuthorRoleId)
        };

        if (entity.PrimarySupervisorId.HasValue)
        {
            targetAssignments.Add((entity.PrimarySupervisorId.Value, PrimarySupervisorRoleId));
        }

        if (entity.ExternalSupervisorId.HasValue)
        {
            targetAssignments.Add((entity.ExternalSupervisorId.Value, ExternalSupervisorRoleId));
        }

        var existingUserProjects = (await _userProjectRepository.AllAsyncByProjectId(dalEntity.Id, userId))
            .Where(up => trackedRoleIds.Contains(up.UserProjectRoleId))
            .ToList();

        var keptAssignments = new HashSet<(Guid UserId, Guid RoleId)>();
        foreach (var userProject in existingUserProjects)
        {
            var key = (userProject.UserId, userProject.UserProjectRoleId);
            if (!targetAssignments.Contains(key) || !keptAssignments.Add(key))
            {
                _userProjectRepository.Remove(userProject.Id);
            }
        }

        foreach (var assignment in targetAssignments.Where(t => !keptAssignments.Contains(t)))
        {
            _userProjectRepository.Add(new DAL.DTO.UserProject()
            {
                ProjectId = dalEntity.Id,
                UserId = assignment.UserId,
                UserProjectRoleId = assignment.RoleId,
            });
        }

        return Mapper.Map(updatedEntity);
    }
}