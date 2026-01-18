using Base.Contracts;
using DAL.DTO;

namespace DAL.EF.Mappers;

public class ProjectFolderUOWMapper : IMapper<DAL.DTO.ProjectFolder, Domain.ProjectFolder>
{
    public ProjectFolder? Map(Domain.ProjectFolder? entity)
    {
        if (entity == null) return null;
        
        return new ProjectFolder()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new ProjectUOWMapper().Map(entity.Project) : null,
            FolderId = entity.FolderId,
            Folder = entity.Folder != null ? new FolderUOWMapper().Map(entity.Folder) : null,
        };
    }

    public Domain.ProjectFolder? Map(ProjectFolder? entity)
    {
        if (entity == null) return null;
        
        return new Domain.ProjectFolder()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new ProjectUOWMapper().Map(entity.Project) : null,
            FolderId = entity.FolderId,
            Folder = entity.Folder != null ? new FolderUOWMapper().Map(entity.Folder) : null,
        };
    }
}