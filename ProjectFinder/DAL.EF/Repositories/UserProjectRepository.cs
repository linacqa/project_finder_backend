using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class UserProjectRepository : BaseRepository<UserProject, Domain.UserProject>, IUserProjectRepository
{
    public UserProjectRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new UserProjectUOWMapper())
    {
    }
}