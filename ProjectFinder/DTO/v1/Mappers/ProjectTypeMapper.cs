using Base.Contracts;

namespace DTO.v1.Mappers;

public class ProjectTypeMapper : IMapper<DTO.v1.ProjectType, BLL.DTO.ProjectType>
{
    public ProjectType? Map(BLL.DTO.ProjectType? entity)
    {
        if (entity == null) return null;
        
        var result = new ProjectType()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }

    public BLL.DTO.ProjectType? Map(ProjectType? entity)
    {
        if (entity == null) return null;
        
        var result = new BLL.DTO.ProjectType()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }

    // public BLL.DTO.ProjectType Map(ProjectTypeCreate entity)
    // {
    //     var result = new BLL.DTO.ProjectType()
    //     {
    //         Id = Guid.NewGuid(),
    //         Name = entity.Name,
    //     };
    //     
    //     return result;
    // }
}