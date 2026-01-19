using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class UserTagService : BaseService<BLL.DTO.UserTag, DAL.DTO.UserTag, DAL.Contracts.IUserTagRepository>, IUserTagService
{
    public UserTagService(
        IAppUOW serviceUOW, 
        IMapper<UserTag, DAL.DTO.UserTag, Guid> mapper) : base(serviceUOW, serviceUOW.UserTagRepository, mapper)
    {
    }
}