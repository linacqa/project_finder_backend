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
        
        Console.WriteLine(dalEntity.AcceptedAt);
        Console.WriteLine(updatedEntity.GroupId);
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
}
