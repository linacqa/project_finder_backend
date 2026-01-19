using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class UserGroupRepository : BaseRepository<UserGroup, Domain.UserGroup>, IUserGroupRepository
{
    public UserGroupRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new UserGroupUOWMapper())
    {
    }
}