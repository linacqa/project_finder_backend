using Base.Contracts;
using DAL.DTO;
using DAL.EF.Mappers.Identity;

namespace DAL.EF.Mappers;

public class InvitationUOWMapper : IMapper<DAL.DTO.Invitation, Domain.Invitation>
{
    public Invitation? Map(Domain.Invitation? entity)
    {
        if (entity == null) return null;
        
        return new Invitation()
        {
            Id = entity.Id,
            GroupId = entity.GroupId,
            Group = entity.Group != null ? new GroupUOWMapper().Map(entity.Group) : null,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new ProjectUOWMapper().Map(entity.Project) : null,
            ToUserId = entity.UserId,
            ToUser = entity.User != null ? new AppUserUOWMapper().Map(entity.User) : null,
            FromUserId = entity.FromUserId,
            FromUser = entity.FromUser != null ? new AppUserUOWMapper().Map(entity.FromUser) : null,
            AcceptedAt = entity.AcceptedAt,
            DeclinedAt = entity.DeclinedAt,
        };
    }

    public Domain.Invitation? Map(Invitation? entity)
    {
        if (entity == null) return null;
        
        return new Domain.Invitation()
        {
            Id = entity.Id,
            GroupId = entity.GroupId,
            Group = entity.Group != null ? new GroupUOWMapper().Map(entity.Group) : null,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new ProjectUOWMapper().Map(entity.Project) : null,
            UserId = entity.ToUserId,
            User = entity.ToUser != null ? new AppUserUOWMapper().Map(entity.ToUser) : null,
            FromUserId = entity.FromUserId,
            FromUser = entity.FromUser != null ? new AppUserUOWMapper().Map(entity.FromUser) : null,
            AcceptedAt = entity.AcceptedAt,
            DeclinedAt = entity.DeclinedAt,
        };
    }
}