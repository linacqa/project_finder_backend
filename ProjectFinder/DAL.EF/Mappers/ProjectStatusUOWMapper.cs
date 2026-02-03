using Base.Contracts;
using DAL.DTO;

namespace DAL.EF.Mappers;

public class ProjectStatusUOWMapper : IMapper<DAL.DTO.ProjectStatus, Domain.ProjectStatus>
{
    public ProjectStatus? Map(Domain.ProjectStatus? entity)
    {
        if (entity == null) return null;
        
        return new ProjectStatus()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }

    public Domain.ProjectStatus? Map(ProjectStatus? entity)
    {
        if (entity == null) return null;
        
        return new Domain.ProjectStatus()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }
}