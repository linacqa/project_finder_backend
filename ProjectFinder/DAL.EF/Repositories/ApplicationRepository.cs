using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class ApplicationRepository : BaseRepository<Application, Domain.Application>, IApplicationRepository
{
    public ApplicationRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new ApplicationUOWMapper())
    {
    }
    
    public async Task<DAL.DTO.Application?> FindAsyncByProjectId(Guid projectId, Guid userId)
    {
        var query = GetQuery(userId);
        var res = await query.FirstOrDefaultAsync(e => e.ProjectId.Equals(projectId));
        return Mapper.Map(res);
    }
}
