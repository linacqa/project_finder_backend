using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class GroupService : BaseService<BLL.DTO.Group, DAL.DTO.Group, DAL.Contracts.IGroupRepository>, IGroupService
{
    private readonly IUserGroupRepository _userGroupRepository;
    
    public GroupService(
        IAppUOW serviceUOW, 
        IMapper<Group, DAL.DTO.Group, Guid> mapper) : base(serviceUOW, serviceUOW.GroupRepository, mapper)
    {
        _userGroupRepository = serviceUOW.UserGroupRepository;
    }
    
    public async Task<IEnumerable<BLL.DTO.Group>> AllAsyncMatchingTeamSize(int minStudents, int maxStudents, Guid userId = default)
    {
        var groups = await ServiceRepository.AllAsyncMatchingTeamSize(minStudents, maxStudents, userId);
        return groups.Select(g => Mapper.Map(g)!);
    }
    
    public override void Add(BLL.DTO.Group entity, Guid userId = default)
    {
        var creatorRoleInGroup = entity.CreatorRoleInGroup;
        
        var dalEntity = Mapper.Map(entity);
        ServiceRepository.Add(dalEntity!, userId);
        
        if (!string.IsNullOrEmpty(creatorRoleInGroup))
        {
            _userGroupRepository.Add(new DAL.DTO.UserGroup()
            {
                UserId = userId,
                GroupId = dalEntity!.Id,
                Role = creatorRoleInGroup
            }, userId);
        }
    }
}