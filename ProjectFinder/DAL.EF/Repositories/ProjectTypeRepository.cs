using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class ProjectTypeRepository : BaseRepository<ProjectType, Domain.ProjectType>, IProjectTypeRepository
{
    public ProjectTypeRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new ProjectTypeUOWMapper())
    {
    }
}