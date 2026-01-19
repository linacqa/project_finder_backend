using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class StepService : BaseService<BLL.DTO.Step, DAL.DTO.Step, DAL.Contracts.IStepRepository>, IStepService
{
    public StepService(
        IAppUOW serviceUOW, 
        IMapper<Step, DAL.DTO.Step, Guid> mapper) : base(serviceUOW, serviceUOW.StepRepository, mapper)
    {
    }
}