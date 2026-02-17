using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class UserProjectRoleRepository : BaseRepository<UserProjectRole, Domain.UserProjectRole>, IUserProjectRoleRepository
{
    public UserProjectRoleRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new UserProjectRoleUOWMapper())
    {
    }
}