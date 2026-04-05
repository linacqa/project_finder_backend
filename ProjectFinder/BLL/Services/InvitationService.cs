using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class InvitationService : BaseService<BLL.DTO.Invitation, DAL.DTO.Invitation, DAL.Contracts.IInvitationRepository>, IInvitationService
{
    private readonly IUserGroupRepository _userGroupRepository;
    
    public InvitationService(
        IAppUOW serviceUOW, 
        IMapper<Invitation, DAL.DTO.Invitation, Guid> mapper) : base(serviceUOW, serviceUOW.InvitationRepository, mapper)
    {
        _userGroupRepository = serviceUOW.UserGroupRepository;
    }
    
    public async Task<IEnumerable<BLL.DTO.Invitation>> AllAsyncToUser(Guid userId = default)
    {
        var entities = await ServiceRepository.AllAsyncToUser(userId);
        return entities.Select(e => Mapper.Map(e)!).ToList(); 
    }
    
    public override async Task<BLL.DTO.Invitation?> UpdateAsync(BLL.DTO.Invitation entity, Guid userId = default)
    {
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
}