using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class ProjectStepRepository : BaseRepository<ProjectStep, Domain.ProjectStep>, IProjectStepRepository
{
    public ProjectStepRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new ProjectStepUOWMapper())
    {
    }
}