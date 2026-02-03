using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class ProjectTypeBLLMapper : IMapper<BLL.DTO.ProjectType, DAL.DTO.ProjectType>
{
    public ProjectType? Map(DAL.DTO.ProjectType? entity)
    {
        if (entity == null) return null;
        
        var result = new ProjectType()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }

    public DAL.DTO.ProjectType? Map(ProjectType? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.ProjectType()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }
}