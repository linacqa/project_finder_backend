using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class GroupRepository : BaseRepository<Group, Domain.Group>, IGroupRepository
{
    public GroupRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new GroupUOWMapper())
    {
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