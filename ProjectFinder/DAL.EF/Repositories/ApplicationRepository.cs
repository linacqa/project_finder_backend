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
        // solo application
        var res = await RepositoryDbSet.AsQueryable()
            .Include(e => e.Group)
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.ProjectId.Equals(projectId) && e.Group == null && e.UserId.Equals(userId));
        
        if (res != null)
            return Mapper.Map(res);
        
        // group application
        var res2 = await RepositoryDbSet.AsQueryable()
            .Include(e => e.Group)
                .ThenInclude(g => g.UserGroups)
                    .ThenInclude(ug => ug.User)
            .FirstOrDefaultAsync(e => e.ProjectId.Equals(projectId) && e.Group != null && e.Group.UserGroups != null && e.Group.UserGroups.Any(ug => ug.UserId.Equals(userId)));
        
        return Mapper.Map(res2);
    }
    
    public override void Add(DAL.DTO.Application entity, Guid userId = default!)
    {
        var dbEntity = Mapper.Map(entity);
        
        dbEntity?.UserId = userId;
        
        RepositoryDbSet.Add(dbEntity!);
    }
}
