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
    
    public override Project? Update(Project entity, Guid userId = default)
    {
        var project = RepositoryDbContext.Set<Domain.Project>().FirstOrDefault(c => c.Id == entity.Id);
        if (project == null || project.UserId != userId)
        {
            throw new UnauthorizedAccessException("The current user does not own the project.");
        }
        
        return base.Update(entity, userId);
    }

    public override Task<Project?> UpdateAsync(Project entity, Guid userId = default)
    {
        var project = RepositoryDbContext.Set<Domain.Project>().FirstOrDefault(c => c.Id == entity.Id);
        if (project == null || project.UserId != userId)
        {
            throw new UnauthorizedAccessException("The current user does not own the project.");
        }
        
        return base.UpdateAsync(entity, userId);
    }

    public override void Remove(Project entity, Guid userId = default)
    {
        var project = RepositoryDbContext.Set<Domain.Project>().FirstOrDefault(c => c.Id == entity.Id);
        if (project == null || project.UserId != userId)
        {
            throw new UnauthorizedAccessException("The current user does not own the project.");
        }
        
        base.Remove(entity, userId);
    }

    public override void Remove(Guid id, Guid userId)
    {
        var project = RepositoryDbContext.Set<Domain.Project>().FirstOrDefault(c => c.Id == id);
        if (project == null || project.UserId != userId)
        {
            throw new UnauthorizedAccessException("The current user does not own the project.");
        }
        
        base.Remove(id, userId);
    }

    public override Task RemoveAsync(Guid id, Guid userId = default)
    {
        var project = RepositoryDbContext.Set<Domain.Project>().FirstOrDefault(c => c.Id == id);
        if (project == null || project.UserId != userId)
        {
            throw new UnauthorizedAccessException("The current user does not own the project.");
        }

        return base.RemoveAsync(id, userId);
    }
}