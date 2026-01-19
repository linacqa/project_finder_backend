using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class ProjectRepository : BaseRepository<Project, Domain.Project>, IProjectRepository
{
    public ProjectRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new ProjectUOWMapper())
    {
    }
}