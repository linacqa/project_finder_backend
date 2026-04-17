using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class ProjectFolderRepository : BaseRepository<ProjectFolder, Domain.ProjectFolder>, IProjectFolderRepository
{
    public ProjectFolderRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new ProjectFolderUOWMapper())
    {
    }

    public async Task<IEnumerable<ProjectFolder>> AllAsyncByProjectId(Guid projectId, Guid userId)
    {
        var res = await RepositoryDbSet.AsQueryable()
            .Where(e => e.ProjectId.Equals(projectId))
            .ToListAsync();

        return res.Select(e => Mapper.Map(e))!;
    }
}