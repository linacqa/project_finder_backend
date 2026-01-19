using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class ApplicationService : BaseService<BLL.DTO.Application, DAL.DTO.Application, DAL.Contracts.IApplicationRepository>, IApplicationService
{
    public ApplicationService(
        IAppUOW serviceUOW, 
        IMapper<Application, DAL.DTO.Application, Guid> mapper) : base(serviceUOW, serviceUOW.ApplicationRepository, mapper)
    {
    }
}
