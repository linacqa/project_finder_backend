using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class GroupProjectService : BaseService<BLL.DTO.GroupProject, DAL.DTO.GroupProject, DAL.Contracts.IGroupProjectRepository>, IGroupProjectService
{
    public GroupProjectService(
        IAppUOW serviceUOW, 
        IMapper<GroupProject, DAL.DTO.GroupProject, Guid> mapper) : base(serviceUOW, serviceUOW.GroupProjectRepository, mapper)
    {
    }
}