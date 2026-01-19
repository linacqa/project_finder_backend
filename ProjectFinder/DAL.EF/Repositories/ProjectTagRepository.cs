using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class ProjectTagRepository : BaseRepository<ProjectTag, Domain.ProjectTag>, IProjectTagRepository
{
    public ProjectTagRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new ProjectTagUOWMapper())
    {
    }
}