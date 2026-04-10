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
}