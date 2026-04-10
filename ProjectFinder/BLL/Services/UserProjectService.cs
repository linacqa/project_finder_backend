using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class UserProjectService : BaseService<BLL.DTO.UserProject, DAL.DTO.UserProject, DAL.Contracts.IUserProjectRepository>, IUserProjectService
{
    public UserProjectService(
        IAppUOW serviceUOW, 
        IMapper<UserProject, DAL.DTO.UserProject, Guid> mapper) : base(serviceUOW, serviceUOW.UserProjectRepository, mapper)
    {
    }
}