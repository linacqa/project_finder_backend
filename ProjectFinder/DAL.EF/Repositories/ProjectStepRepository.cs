using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class ProjectStepRepository : BaseRepository<ProjectStep, Domain.ProjectStep>, IProjectStepRepository
{
    public ProjectStepRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new ProjectStepUOWMapper())
    {
    }
    
    public async Task<IEnumerable<DAL.DTO.ProjectStep>> AllAsyncByProjectId(Guid projectId, Guid userId)
    {
        var res = await RepositoryDbSet.AsQueryable()
            .Where(e => e.ProjectId.Equals(projectId))
            .Include(e => e.Step)
            .Include(e => e.StepStatus)
            .OrderBy(e => e.Order)
            .ToListAsync();

        return res.Select(e => Mapper.Map(e))!;
    }
}