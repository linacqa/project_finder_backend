using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class InvitationService : BaseService<BLL.DTO.Invitation, DAL.DTO.Invitation, DAL.Contracts.IInvitationRepository>, IInvitationService
{
    private readonly IUserGroupRepository _userGroupRepository;
    private readonly IGroupRepository _groupRepository;
    
    public InvitationService(
        IAppUOW serviceUOW, 
        IMapper<Invitation, DAL.DTO.Invitation, Guid> mapper) : base(serviceUOW, serviceUOW.InvitationRepository, mapper)
    {
        _userGroupRepository = serviceUOW.UserGroupRepository;
        _groupRepository = serviceUOW.GroupRepository;
    }
    
    public async Task<IEnumerable<BLL.DTO.Invitation>> AllAsyncToUser(Guid userId = default)
    {
        var entities = await ServiceRepository.AllAsyncToUser(userId);
        return entities.Select(e => Mapper.Map(e)!).ToList(); 
    }
    
    public async Task<IEnumerable<BLL.DTO.Invitation>> AllAsyncByGroupId(Guid groupId, Guid userId = default)
    {
        var group  = await _groupRepository.FindAsync(groupId);
        if (group == null)
        {
            throw new InvalidOperationException("Group not found.");
        }

        if (!_userGroupRepository.UserInGroup(userId, groupId))
        {
            throw new UnauthorizedAccessException("Only group members can see invitations.");
        }
        var entities = await ServiceRepository.AllAsyncByGroupId(groupId, userId);
        return entities.Select(e => Mapper.Map(e)!).ToList(); 
    }
    
    public override void Add(BLL.DTO.Invitation entity, Guid userId = default)
    {
        var group = _groupRepository.Find(entity.GroupId);
        if (group == null)
        {
            throw new InvalidOperationException("Group not found.");
        }
        if (group.CreatorId != userId)
        {
            throw new UnauthorizedAccessException("Only group creator can send invitations.");
        }
        if (group.UserGroups.Count >= 10)
        {
            throw new InvalidOperationException("Cannot add more than 10 members.");
        }
        if (_userGroupRepository.UserInGroup(entity.ToUserId, entity.GroupId))
        {
            throw new InvalidOperationException("Invitable user is already a member of this group.");
        }
        
        var dalEntity = Mapper.Map(entity);
        ServiceRepository.Add(dalEntity!, userId);
    }
    
    public override async Task<BLL.DTO.Invitation?> UpdateAsync(BLL.DTO.Invitation entity, Guid userId = default)
    {
        if (entity.ToUserId != userId)
        {
            throw new UnauthorizedAccessException("You can only accept/decline your own invitations.");
        }
        if (entity.AcceptedAt != null && _userGroupRepository.UserInGroup(entity.ToUserId, entity.GroupId))
        {
            throw new InvalidOperationException("You are already a member of this group.");
        }
        var group = await _groupRepository.FindAsync(entity.GroupId);
        if (group.UserGroups.Count >= 10 && entity.AcceptedAt != null)
        {
            throw new InvalidOperationException("Group cannot have more than 10 members.");
        }
        
        var dalEntity = Mapper.Map(entity);
        var updatedEntity = await ServiceRepository.UpdateAsync(dalEntity!, userId);
        if (updatedEntity.AcceptedAt != null)
        {
            _userGroupRepository.Add(new DAL.DTO.UserGroup()
            {
                UserId = updatedEntity.ToUserId,
                GroupId = updatedEntity.GroupId,
                Role = updatedEntity.Role,
            }, userId);
        }
        
        return Mapper.Map(updatedEntity);
    }
    
    public override void Remove(BLL.DTO.Invitation entity, Guid userId = default)
    {
        Remove(entity.Id, userId);
    }

    public override void Remove(Guid id, Guid userId = default)
    {
        var entity = ServiceRepository.Find(id, userId);
        if (entity.FromUserId != userId)
        {
            throw new UnauthorizedAccessException("User is not the sender of the invitation.");
        }
        if (entity.AcceptedAt != null)
        {
            throw new InvalidOperationException("Cannot remove an accepted invitation.");
        }
        if (entity != null)
        {
            ServiceRepository.Remove(entity, userId);
        }
    }

    public override async Task RemoveAsync(Guid id, Guid userId = default)
    {
        var entity = await ServiceRepository.FindAsync(id, userId);
        if (entity.FromUserId != userId)
        {
            throw new UnauthorizedAccessException("User is not the sender of the invitation.");
        }

        if (entity.AcceptedAt != null)
        {
            throw new InvalidOperationException("Cannot remove an accepted invitation.");
        }
        if (entity != null)
        {
            await ServiceRepository.RemoveAsync(id, userId);
        }
    }
}