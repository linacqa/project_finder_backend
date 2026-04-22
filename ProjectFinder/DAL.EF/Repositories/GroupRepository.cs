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

    public async Task<IEnumerable<Group>> AllAsyncMatchingTeamSize(int minStudents, int maxStudents,
        Guid userId = default)
    {
        return (await GetQuery(userId)
                .Where(e => !e.IsAzureAdGroup)
                .Include(e => e.UserGroups)!
                    .ThenInclude(u => u.User)
                .Where(e => e.UserGroups != null && e.UserGroups.Count() >= minStudents && e.UserGroups.Count() <= maxStudents)
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
        Remove(entity.Id, userId);
    }

    public override void Remove(Guid id, Guid userId)
    {
        var group = RepositoryDbContext.Set<Domain.Group>().FirstOrDefault(c => c.Id == id);
        if (group == null || group.UserId != userId)
        {
            throw new UnauthorizedAccessException("The current user does not own the group.");
        }

        DeleteGroupDependencies(id);
        base.Remove(id, userId);
    }

    public override async Task RemoveAsync(Guid id, Guid userId = default)
    {
        var group = await RepositoryDbContext.Set<Domain.Group>().FirstOrDefaultAsync(c => c.Id == id);
        if (group == null || group.UserId != userId)
        {
            throw new UnauthorizedAccessException("The current user does not own the group.");
        }

        await DeleteGroupDependenciesAsync(id);
        await base.RemoveAsync(id, userId);
    }

    private void DeleteGroupDependencies(Guid groupId)
    {
        var invitations = RepositoryDbContext.Set<Domain.Invitation>()
            .Where(i => i.GroupId == groupId);
        var applications = RepositoryDbContext.Set<Domain.Application>()
            .Where(a => a.GroupId == groupId);
        var userGroups = RepositoryDbContext.Set<Domain.UserGroup>()
            .Where(ug => ug.GroupId == groupId);

        RepositoryDbContext.RemoveRange(invitations);
        RepositoryDbContext.RemoveRange(applications);
        RepositoryDbContext.RemoveRange(userGroups);
    }

    private async Task DeleteGroupDependenciesAsync(Guid groupId)
    {
        var invitations = await RepositoryDbContext.Set<Domain.Invitation>()
            .Where(i => i.GroupId == groupId)
            .ToListAsync();
        var applications = await RepositoryDbContext.Set<Domain.Application>()
            .Where(a => a.GroupId == groupId)
            .ToListAsync();
        var userGroups = await RepositoryDbContext.Set<Domain.UserGroup>()
            .Where(ug => ug.GroupId == groupId)
            .ToListAsync();

        RepositoryDbContext.RemoveRange(invitations);
        RepositoryDbContext.RemoveRange(applications);
        RepositoryDbContext.RemoveRange(userGroups);
    }
}