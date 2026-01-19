using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class ProjectTagBLLMapper : IMapper<BLL.DTO.ProjectTag, DAL.DTO.ProjectTag>
{
    public ProjectTag? Map(DAL.DTO.ProjectTag? entity)
    {
        if (entity == null) return null;
        
        var result = new ProjectTag()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            TagId = entity.TagId,
        };
        
        return result;
    }

    public DAL.DTO.ProjectTag? Map(ProjectTag? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.ProjectTag()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            TagId = entity.TagId,
        };
        
        return result;
    }
}