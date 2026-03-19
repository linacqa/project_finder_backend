using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class GroupRepository : BaseRepository<Group, Domain.Group>, IGroupRepository
{
    public GroupRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new GroupUOWMapper())
    {
    }
    
    public override IEnumerable<Group> All(Guid userId = default!)
    {
        return GetQuery(userId)
            .Where(e => !e.IsAzureAdGroup)
            .Include(e => e.UserGroups)!
                .ThenInclude(u => u.User)
            .ToList()
            .Select(e => Mapper.Map(e)!);
    }

    public override async Task<IEnumerable<Group>> AllAsync(Guid userId = default!)
    {
        return (await GetQuery(userId)
                .Where(e => !e.IsAzureAdGroup)
                .Include(e => e.UserGroups)!
                    .ThenInclude(u => u.User)
                .ToListAsync())
            .Select(e => Mapper.Map(e)!);
    }

    public override Group? Find(Guid id, Guid userId = default)
    {
        var query = GetQuery(userId)
            .Where(e => !e.IsAzureAdGroup)
            .Include(e => e.UserGroups)!
            .ThenInclude(u => u.User);
        var res = query.FirstOrDefault(e => e.Id.Equals(id));
        return Mapper.Map(res);
    }

    public override async Task<Group?> FindAsync(Guid id, Guid userId = default)
    {
        var query = GetQuery(userId)
            .Where(e => !e.IsAzureAdGroup)
            .Include(e => e.UserGroups)!
            .ThenInclude(u => u.User);
        var res = await query.FirstOrDefaultAsync(e => e.Id.Equals(id));
        return Mapper.Map(res);
    }

    public override Group? Update(Group entity, Guid userId = default)
    {
        var group = RepositoryDbContext.Set<Domain.Group>().FirstOrDefault(c => c.Id == entity.Id);
        if (group == null || group.UserId != userId)
        {
            throw new UnauthorizedAccessException("The current user does not own the group.");
        }
        
        return base.Update(entity, userId);
    }

    public override Task<Group?> UpdateAsync(Group entity, Guid userId = default)
    {
        var group = RepositoryDbContext.Set<Domain.Group>().FirstOrDefault(c => c.Id == entity.Id);
        if (group == null || group.UserId != userId)
        {
            throw new UnauthorizedAccessException("The current user does not own the group.");
        }
        
        return base.UpdateAsync(entity, userId);
    }

    public override void Remove(Group entity, Guid userId = default)
    {
        var group = RepositoryDbContext.Set<Domain.Group>().FirstOrDefault(c => c.Id == entity.Id);
        if (group == null || group.UserId != userId)
        {
            throw new UnauthorizedAccessException("The current user does not own the group.");
        }
        
        base.Remove(entity, userId);
    }

    public override void Remove(Guid id, Guid userId)
    {
        var group = RepositoryDbContext.Set<Domain.Group>().FirstOrDefault(c => c.Id == id);
        if (group == null || group.UserId != userId)
        {
            throw new UnauthorizedAccessException("The current user does not own the group.");
        }
        
        base.Remove(id, userId);
    }

    public override Task RemoveAsync(Guid id, Guid userId = default)
    {
        var group = RepositoryDbContext.Set<Domain.Group>().FirstOrDefault(c => c.Id == id);
        if (group == null || group.UserId != userId)
        {
            throw new UnauthorizedAccessException("The current user does not own the group.");
        }

        return base.RemoveAsync(id, userId);
    }
}