using Base.DAL.Contracts;
using DAL.DTO;

namespace DAL.Contracts;

public interface IApplicationRepository : IBaseRepository<Application>
{
    Task<DAL.DTO.Application?> FindAsyncByProjectId(Guid projectId, Guid userId);
}