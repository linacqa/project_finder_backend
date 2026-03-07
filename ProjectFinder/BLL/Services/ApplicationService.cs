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

    public async Task<BLL.DTO.Application?> FindAsyncByProjectId(Guid projectId, Guid userId)
    {
        var entity = await ServiceRepository.FindAsyncByProjectId(projectId, userId);
        return Mapper.Map(entity);
    }
}
