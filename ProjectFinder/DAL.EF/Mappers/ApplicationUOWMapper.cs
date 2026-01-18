using Base.Contracts;
using DAL.DTO;
using DAL.EF.Mappers.Identity;

namespace DAL.EF.Mappers;

public class ApplicationUOWMapper : IMapper<DAL.DTO.Application, Domain.Application>
{
    public Application? Map(Domain.Application? entity)
    {
        if (entity == null) return null;

        return new Application()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            User = entity.User != null ? new AppUserUOWMapper().Map(entity.User) : null,
            GroupId = entity.GroupId,
            Group = entity.Group != null ? new GroupUOWMapper().Map(entity.Group) : null,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new ProjectUOWMapper().Map(entity.Project) : null,
            AcceptedAt = entity.AcceptedAt,
            DeclinedAt = entity.DeclinedAt,
        };
    }

    public Domain.Application? Map(Application? entity)
    {
        if (entity == null) return null;
        
        return new Domain.Application()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            User = entity.User != null ? new AppUserUOWMapper().Map(entity.User) : null,
            GroupId = entity.GroupId,
            Group = entity.Group != null ? new GroupUOWMapper().Map(entity.Group) : null,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new ProjectUOWMapper().Map(entity.Project) : null,
            AcceptedAt = entity.AcceptedAt,
            DeclinedAt = entity.DeclinedAt,
        };
    }
}