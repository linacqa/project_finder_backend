using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class ProjectRepository : BaseRepository<Project, Domain.Project>, IProjectRepository
{
    public ProjectRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new ProjectUOWMapper())
    {
    }

    public override async Task<IEnumerable<Project>> AllAsync(Guid userId = default)
    {
        return (await GetQuery(userId)
                .Include(p => p.ProjectType)
                .Include(p => p.ProjectStatus)
                .Include(p => p.ProjectTags)!
                    .ThenInclude(pt => pt.Tag)
                .Include(p => p.UserProjects)!
                    .ThenInclude(up => up.User)
                .Include(p => p.UserProjects)!
                    .ThenInclude(up => up.UserProjectRole)
                .ToListAsync())
            .Select(e => Mapper.Map(e)!);
    }

    public override IEnumerable<Project> All(Guid userId = default)
    {
        return GetQuery(userId)
            .Include(p => p.ProjectType)
            .Include(p => p.ProjectStatus)
            .Include(p => p.ProjectTags)!
                .ThenInclude(pt => pt.Tag)
            .Include(p => p.UserProjects)!
            .ThenInclude(up => up.User)
            .Include(p => p.UserProjects)!
            .ThenInclude(up => up.UserProjectRole)
            .ToList()
            .Select(e => Mapper.Map(e)!);
    }

    public override async Task<Project?> FindAsync(Guid id, Guid userId = default)
    {
        var res = await GetQuery(userId)
            .Include(p => p.ProjectType)
            .Include(p => p.ProjectStatus)
            .Include(p => p.ProjectTags)!
            .ThenInclude(pt => pt.Tag)
            .Include(p => p.UserProjects)!
            .ThenInclude(up => up.User)
            .Include(p => p.UserProjects)!
            .ThenInclude(up => up.UserProjectRole)
            .FirstOrDefaultAsync(e => e.Id.Equals(id));
        
        return Mapper.Map(res);
    }

    public override Project? Find(Guid id, Guid userId = default)
    {
        var res = GetQuery(userId)
            .Include(p => p.ProjectType)
            .Include(p => p.ProjectStatus)
            .Include(p => p.ProjectTags)!
            .ThenInclude(pt => pt.Tag)
            .Include(p => p.UserProjects)!
            .ThenInclude(up => up.User)
            .Include(p => p.UserProjects)!
            .ThenInclude(up => up.UserProjectRole)
            .FirstOrDefault(e => e.Id.Equals(id));
        
        return Mapper.Map(res);
    }

    // public override Project? Update(Project entity, Guid userId = default)
    // {
    //     var project = RepositoryDbContext.Set<Domain.Project>().FirstOrDefault(c => c.Id == entity.Id);
    //     if (project == null || project.UserId != userId)
    //     {
    //         throw new UnauthorizedAccessException("The current user does not own the project.");
    //     }
    //     
    //     return base.Update(entity, userId);
    // }
    //
    // public override Task<Project?> UpdateAsync(Project entity, Guid userId = default)
    // {
    //     var project = RepositoryDbContext.Set<Domain.Project>().FirstOrDefault(c => c.Id == entity.Id);
    //     if (project == null || project.UserId != userId)
    //     {
    //         throw new UnauthorizedAccessException("The current user does not own the project.");
    //     }
    //     
    //     return base.UpdateAsync(entity, userId);
    // }
    //
    // public override void Remove(Project entity, Guid userId = default)
    // {
    //     var project = RepositoryDbContext.Set<Domain.Project>().FirstOrDefault(c => c.Id == entity.Id);
    //     if (project == null || project.UserId != userId)
    //     {
    //         throw new UnauthorizedAccessException("The current user does not own the project.");
    //     }
    //     
    //     base.Remove(entity, userId);
    // }
    //
    // public override void Remove(Guid id, Guid userId)
    // {
    //     var project = RepositoryDbContext.Set<Domain.Project>().FirstOrDefault(c => c.Id == id);
    //     if (project == null || project.UserId != userId)
    //     {
    //         throw new UnauthorizedAccessException("The current user does not own the project.");
    //     }
    //     
    //     base.Remove(id, userId);
    // }
    //
    // public override Task RemoveAsync(Guid id, Guid userId = default)
    // {
    //     var project = RepositoryDbContext.Set<Domain.Project>().FirstOrDefault(c => c.Id == id);
    //     if (project == null || project.UserId != userId)
    //     {
    //         throw new UnauthorizedAccessException("The current user does not own the project.");
    //     }
    //
    //     return base.RemoveAsync(id, userId);
    // }
}