using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class ProjectTagRepository : BaseRepository<ProjectTag, Domain.ProjectTag>, IProjectTagRepository
{
    public ProjectTagRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new ProjectTagUOWMapper())
    {
    }

    public async Task<IEnumerable<ProjectTag>> AllAsyncByProjectId(Guid projectId, Guid userId)
    {
        var res = await RepositoryDbSet.AsQueryable()
            .Where(e => e.ProjectId.Equals(projectId))
            .ToListAsync();

        return res.Select(e => Mapper.Map(e))!;
    }
}