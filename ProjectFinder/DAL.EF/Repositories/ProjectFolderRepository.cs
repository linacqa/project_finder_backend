using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class ProjectFolderRepository : BaseRepository<ProjectFolder, Domain.ProjectFolder>, IProjectFolderRepository
{
    public ProjectFolderRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new ProjectFolderUOWMapper())
    {
    }
}