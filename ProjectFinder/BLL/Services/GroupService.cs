using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class GroupService : BaseService<BLL.DTO.Group, DAL.DTO.Group, DAL.Contracts.IGroupRepository>, IGroupService
{
    public GroupService(
        IAppUOW serviceUOW, 
        IMapper<Group, DAL.DTO.Group, Guid> mapper) : base(serviceUOW, serviceUOW.GroupRepository, mapper)
    {
    }
}