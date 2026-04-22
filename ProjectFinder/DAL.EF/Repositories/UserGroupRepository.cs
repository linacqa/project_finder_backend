using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class UserGroupRepository : BaseRepository<UserGroup, Domain.UserGroup>, IUserGroupRepository
{
    public UserGroupRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new UserGroupUOWMapper())
    {
    }
    
    public bool UserInGroup(Guid userId, Guid groupId)
    {
        return RepositoryDbContext.Set<Domain.UserGroup>().Any(ug => ug.UserId == userId && ug.GroupId == groupId);
    }

    public override void Remove(UserGroup entity, Guid userId = default)
    {
        Remove(entity.Id, userId);
    }

    public override void Remove(Guid id, Guid userId)
    {
        var query = RepositoryDbSet.AsQueryable();
        query = query.Where(e => e.Id.Equals(id));
        var dbEntity = query.FirstOrDefault();
        if (dbEntity != null)
        {
            RepositoryDbSet.Remove(dbEntity);
        }
    }

    public override async Task RemoveAsync(Guid id, Guid userId = default)
    {
        var query = RepositoryDbSet.AsQueryable();
        query = query.Where(e => e.Id.Equals(id));
        var dbEntity = await query.FirstOrDefaultAsync();
        if (dbEntity != null)
        {
            RepositoryDbSet.Remove(dbEntity);
        }
    }
}