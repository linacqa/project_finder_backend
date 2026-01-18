using Base.Contracts;
using DAL.DTO;

namespace DAL.EF.Mappers;

public class GroupProjectUOWMapper : IMapper<DAL.DTO.GroupProject, Domain.GroupProject>
{
    public GroupProject? Map(Domain.GroupProject? entity)
    {
        if (entity == null) return null;
        
        return new GroupProject()
        {
            Id = entity.Id,
            GroupId = entity.GroupId,
            Group = entity.Group != null ? new GroupUOWMapper().Map(entity.Group) : null,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new ProjectUOWMapper().Map(entity.Project) : null,
        };
    }

    public Domain.GroupProject? Map(GroupProject? entity)
    {
        if (entity == null) return null;
        
        return new Domain.GroupProject()
        {
            Id = entity.Id,
            GroupId = entity.GroupId,
            Group = entity.Group != null ? new GroupUOWMapper().Map(entity.Group) : null,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new ProjectUOWMapper().Map(entity.Project) : null,
        };
    }
}