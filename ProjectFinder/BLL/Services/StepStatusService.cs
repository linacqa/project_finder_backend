using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class StepStatusService : BaseService<BLL.DTO.StepStatus, DAL.DTO.StepStatus, DAL.Contracts.IStepStatusRepository>, IStepStatusService
{
    public StepStatusService(
        IAppUOW serviceUOW, 
        IMapper<StepStatus, DAL.DTO.StepStatus, Guid> mapper) : base(serviceUOW, serviceUOW.StepStatusRepository, mapper)
    {
    }
}