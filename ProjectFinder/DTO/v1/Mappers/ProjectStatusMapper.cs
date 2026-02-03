using Base.Contracts;

namespace DTO.v1.Mappers;

public class ProjectStatusMapper : IMapper<DTO.v1.ProjectStatus, BLL.DTO.ProjectStatus>
{
    public ProjectStatus? Map(BLL.DTO.ProjectStatus? entity)
    {
        if (entity == null) return null;
        
        var result = new ProjectStatus()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }

    public BLL.DTO.ProjectStatus? Map(ProjectStatus? entity)
    {
        if (entity == null) return null;
        
        var result = new BLL.DTO.ProjectStatus()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }

    // public BLL.DTO.ProjectStatus Map(ProjectStatusCreate entity)
    // {
    //     var result = new BLL.DTO.ProjectStatus()
    //     {
    //         Id = Guid.NewGuid(),
    //         Name = entity.Name,
    //     };
    //     
    //     return result;
    // }
}