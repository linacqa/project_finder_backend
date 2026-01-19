using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class UserTagRepository : BaseRepository<UserTag, Domain.UserTag>, IUserTagRepository
{
    public UserTagRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new UserTagUOWMapper())
    {
    }
}