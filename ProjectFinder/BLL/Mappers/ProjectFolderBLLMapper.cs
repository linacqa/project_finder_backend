using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class ProjectFolderBLLMapper : IMapper<BLL.DTO.ProjectFolder, DAL.DTO.ProjectFolder>
{
    public ProjectFolder? Map(DAL.DTO.ProjectFolder? entity)
    {
        if (entity == null) return null;
        
        var result = new ProjectFolder()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            FolderId = entity.FolderId,
        };
        
        return result;
    }

    public DAL.DTO.ProjectFolder? Map(ProjectFolder? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.ProjectFolder()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            FolderId = entity.FolderId,
        };
        
        return result;
    }
}