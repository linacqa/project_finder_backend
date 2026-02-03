using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class ProjectStatusRepository : BaseRepository<ProjectStatus, Domain.ProjectStatus>, IProjectStatusRepository
{
    public ProjectStatusRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new ProjectStatusUOWMapper())
    {
    }
}