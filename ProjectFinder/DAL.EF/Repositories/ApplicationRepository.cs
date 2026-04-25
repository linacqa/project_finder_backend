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
    
    public override IEnumerable<DAL.DTO.Application> All(Guid userId = default!)
    {
        IQueryable<Domain.Application> query = GetQuery(userId)
            .Include(e => e.Group)
            .ThenInclude(g => g.UserGroups)
            .Include(e => e.User)
            .Include(e => e.Project);

        if (userId != default)
        {
            query = query.Where(e => e.UserId.Equals(userId) || (e.Group != null && e.Group.UserGroups != null && e.Group.UserGroups.Any(ug => ug.UserId.Equals(userId))));
        }
        return query.ToList()
            .Select(e => Mapper.Map(e)!);
    }

    public override async Task<IEnumerable<DAL.DTO.Application>> AllAsync(Guid userId = default!)
    {
        IQueryable<Domain.Application> query = GetQuery(userId)
            .Include(e => e.Group)
            .ThenInclude(g => g.UserGroups)
            .Include(e => e.User)
            .Include(e => e.Project);
        if (userId != default)  
        {
            query = query.Where(e => e.UserId.Equals(userId) || (e.Group != null && e.Group.UserGroups != null && e.Group.UserGroups.Any(ug => ug.UserId.Equals(userId))));
        }
        return (await query.ToListAsync())
            .Select(e => Mapper.Map(e)!);
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
    
    public override async Task<DAL.DTO.Application?> UpdateAsync(DAL.DTO.Application entity, Guid userId = default)
    {
        var result = await base.UpdateAsync(entity, userId);
        if (result == null) return null;
        
        // Reload with includes to ensure navigation properties are loaded
        var domainEntity = await RepositoryDbSet.AsQueryable()
            .Include(e => e.Group)
                .ThenInclude(g => g.UserGroups)
            .Include(e => e.User)
            .Include(e => e.Project)
            .FirstOrDefaultAsync(e => e.Id.Equals(entity.Id));
        
        return Mapper.Map(domainEntity);
    }
    
    public override void Add(DAL.DTO.Application entity, Guid userId = default!)
    {
        var dbEntity = Mapper.Map(entity);
        
        dbEntity?.UserId = userId;
        
        RepositoryDbSet.Add(dbEntity!);
    }
}
