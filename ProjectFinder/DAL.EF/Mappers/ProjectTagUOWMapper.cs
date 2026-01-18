using Base.Contracts;
using DAL.DTO;

namespace DAL.EF.Mappers;

public class ProjectTagUOWMapper : IMapper<DAL.DTO.ProjectTag, Domain.ProjectTag>
{
    public ProjectTag? Map(Domain.ProjectTag? entity)
    {
        if (entity == null) return null;
        
        return new ProjectTag()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new ProjectUOWMapper().Map(entity.Project) : null,
            TagId = entity.TagId,
            Tag = entity.Tag != null ? new TagUOWMapper().Map(entity.Tag) : null
        };
    }

    public Domain.ProjectTag? Map(ProjectTag? entity)
    {
        if (entity == null) return null;
        
        return new Domain.ProjectTag()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new ProjectUOWMapper().Map(entity.Project) : null,
            TagId = entity.TagId,
            Tag = entity.Tag != null ? new TagUOWMapper().Map(entity.Tag) : null
        };
    }
}