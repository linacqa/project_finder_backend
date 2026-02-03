using Base.Contracts;
using DAL.DTO;

namespace DAL.EF.Mappers;

public class ProjectTypeUOWMapper : IMapper<DAL.DTO.ProjectType, Domain.ProjectType>
{
    public ProjectType? Map(Domain.ProjectType? entity)
    {
        if (entity == null) return null;
        
        return new ProjectType()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }

    public Domain.ProjectType? Map(ProjectType? entity)
    {
        if (entity == null) return null;
        
        return new Domain.ProjectType()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }
}