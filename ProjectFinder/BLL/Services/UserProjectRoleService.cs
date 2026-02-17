using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class UserProjectRoleService : BaseService<BLL.DTO.UserProjectRole, DAL.DTO.UserProjectRole, DAL.Contracts.IUserProjectRoleRepository>, IUserProjectRoleService
{
    public UserProjectRoleService(
        IAppUOW serviceUOW, 
        IMapper<UserProjectRole, DAL.DTO.UserProjectRole, Guid> mapper) : base(serviceUOW, serviceUOW.UserProjectRoleRepository, mapper)
    {
    }
}