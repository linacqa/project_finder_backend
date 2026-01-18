using Base.Contracts;
using DAL.DTO;
using DAL.EF.Mappers.Identity;

namespace DAL.EF.Mappers;

public class UserGroupUOWMapper : IMapper<DAL.DTO.UserGroup, Domain.UserGroup>
{
    public UserGroup? Map(Domain.UserGroup? entity)
    {
        if (entity == null) return null;
        
        return new UserGroup()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            User = entity.User != null ? new AppUserUOWMapper().Map(entity.User) : null,
            GroupId = entity.GroupId,
            Group = entity.Group != null ? new GroupUOWMapper().Map(entity.Group) : null,
        };
    }

    public Domain.UserGroup? Map(UserGroup? entity)
    {
        if (entity == null) return null;
        
        return new Domain.UserGroup()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            User = entity.User != null ? new AppUserUOWMapper().Map(entity.User) : null,
            GroupId = entity.GroupId,
            Group = entity.Group != null ? new GroupUOWMapper().Map(entity.Group) : null,
        };
    }
}