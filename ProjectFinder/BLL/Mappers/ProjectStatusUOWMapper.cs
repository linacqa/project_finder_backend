using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class ProjectStatusBLLMapper : IMapper<BLL.DTO.ProjectStatus, DAL.DTO.ProjectStatus>
{
    public ProjectStatus? Map(DAL.DTO.ProjectStatus? entity)
    {
        if (entity == null) return null;
        
        var result = new ProjectStatus()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }

    public DAL.DTO.ProjectStatus? Map(ProjectStatus? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.ProjectStatus()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }
}