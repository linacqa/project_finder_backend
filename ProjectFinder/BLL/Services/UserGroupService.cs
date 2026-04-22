using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class UserGroupService : BaseService<BLL.DTO.UserGroup, DAL.DTO.UserGroup, DAL.Contracts.IUserGroupRepository>, IUserGroupService
{
    private readonly IGroupRepository _groupRepository;
    
    public UserGroupService(
        IAppUOW serviceUOW, 
        IMapper<UserGroup, DAL.DTO.UserGroup, Guid> mapper) : base(serviceUOW, serviceUOW.UserGroupRepository, mapper)
    {
        _groupRepository = serviceUOW.GroupRepository;
    }
    
    public override void Remove(BLL.DTO.UserGroup entity, Guid userId = default)
    {
        Remove(entity.Id, userId);
    }
    
    public override void Remove(Guid id, Guid userId = default)
    {
        var entity = ServiceRepository.Find(id);
        var group = _groupRepository.Find(entity.GroupId);
        if (group.CreatorId != userId)
        {
            throw new UnauthorizedAccessException("User is not the creator of group.");
        }
        if (entity.UserId == userId)
        {
            throw new InvalidOperationException("Cannot remove yourself from group.");
        }
        if (entity != null)
        {
            ServiceRepository.Remove(entity, userId);
        }
    }
    
    public override async Task RemoveAsync(Guid id, Guid userId = default)
    {
        var entity = await ServiceRepository.FindAsync(id);
        var group = await _groupRepository.FindAsync(entity.GroupId);
        if (group.CreatorId != userId)
        {
            throw new UnauthorizedAccessException("User is not the creator of group.");
        }

        if (entity.UserId == userId)
        {
            throw new InvalidOperationException("Cannot remove yourself from group.");
        }
        if (entity != null)
        {
            await ServiceRepository.RemoveAsync(id, userId);
        }
    }
}