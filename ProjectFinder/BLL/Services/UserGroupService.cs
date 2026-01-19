using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class UserGroupService : BaseService<BLL.DTO.UserGroup, DAL.DTO.UserGroup, DAL.Contracts.IUserGroupRepository>, IUserGroupService
{
    public UserGroupService(
        IAppUOW serviceUOW, 
        IMapper<UserGroup, DAL.DTO.UserGroup, Guid> mapper) : base(serviceUOW, serviceUOW.UserGroupRepository, mapper)
    {
    }
}