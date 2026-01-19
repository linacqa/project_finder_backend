using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class GroupProjectRepository : BaseRepository<GroupProject, Domain.GroupProject>, IGroupProjectRepository
{
    public GroupProjectRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new GroupProjectUOWMapper())
    {
    }
}