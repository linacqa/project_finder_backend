using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class ApplicationService : BaseService<BLL.DTO.Application, DAL.DTO.Application, DAL.Contracts.IApplicationRepository>, IApplicationService
{
    private readonly IUserProjectRepository _userProjectRepository;
    
    public ApplicationService(
        IAppUOW serviceUOW, 
        IMapper<Application, DAL.DTO.Application, Guid> mapper) : base(serviceUOW, serviceUOW.ApplicationRepository, mapper)
    {
        _userProjectRepository = serviceUOW.UserProjectRepository;
    }

    public async Task<BLL.DTO.Application?> FindAsyncByProjectId(Guid projectId, Guid userId)
    {
        var entity = await ServiceRepository.FindAsyncByProjectId(projectId, userId);
        return Mapper.Map(entity);
    }
    
    public override async Task<BLL.DTO.Application?> UpdateAsync(BLL.DTO.Application entity, Guid userId = default)
    {
        var dalEntity = Mapper.Map(entity);
        var updatedEntity = await ServiceRepository.UpdateAsync(dalEntity!, userId);
        
        if (dalEntity.AcceptedAt != null)
        {
            if (updatedEntity.GroupId != null)
            {
                var group = updatedEntity.Group;
                foreach (var userGroup in group.UserGroups)
                {
                    _userProjectRepository.Add(new DAL.DTO.UserProject()
                    {
                        UserId = userGroup.UserId,
                        ProjectId = updatedEntity.ProjectId,
                        UserProjectRoleId = Guid.Parse("00000000-0000-0000-0000-000000000004"), // executor
                    }, userGroup.UserId);
                }
            }
            else
            {
                _userProjectRepository.Add(new DAL.DTO.UserProject()
                {
                    UserId = updatedEntity.UserId.Value,
                    ProjectId = updatedEntity.ProjectId,
                    UserProjectRoleId = Guid.Parse("00000000-0000-0000-0000-000000000004"), // executor
                }, updatedEntity.UserId.Value);
            }
        }
        
        return Mapper.Map(updatedEntity);
    }

    public async Task AddWithValidationAsync(Application entity, Guid userId = default)
    {
        // TODO: check that the project status is open
        var existing = await FindAsyncByProjectId(entity.ProjectId, userId);
        if (existing != null)
        {
            throw new InvalidOperationException("User has already applied to this project.");
        }
        base.Add(entity, userId);
    }

    public override void Remove(BLL.DTO.Application entity, Guid userId = default)
    {
        Remove(entity.Id, userId);
    }

    public override void Remove(Guid id, Guid userId = default)
    {
        var entity = ServiceRepository.Find(id, userId);
        if (entity.UserId != userId)
        {
            throw new UnauthorizedAccessException("User is not the owner of the application.");
        }
        if (entity.AcceptedAt != null)
        {
            throw new InvalidOperationException("Cannot remove an accepted application.");
        }
        if (entity != null)
        {
            ServiceRepository.Remove(entity, userId);
        }
    }

    public override async Task RemoveAsync(Guid id, Guid userId = default)
    {
        var entity = await ServiceRepository.FindAsync(id, userId);
        if (entity.UserId != userId)
        {
            throw new UnauthorizedAccessException("User is not the owner of the application.");
        }

        if (entity.AcceptedAt != null)
        {
            throw new InvalidOperationException("Cannot remove an accepted application.");
        }
        if (entity != null)
        {
            await ServiceRepository.RemoveAsync(id, userId);
        }
    }
}
