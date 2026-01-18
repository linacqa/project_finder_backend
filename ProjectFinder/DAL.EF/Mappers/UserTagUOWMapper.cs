using Base.Contracts;
using DAL.DTO;
using DAL.EF.Mappers.Identity;

namespace DAL.EF.Mappers;

public class UserTagUOWMapper : IMapper<DAL.DTO.UserTag, Domain.UserTag>
{
    public UserTag? Map(Domain.UserTag? entity)
    {
        if (entity == null) return null;
        
        return new UserTag()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            User = entity.User != null ? new AppUserUOWMapper().Map(entity.User) : null,
            TagId = entity.TagId,
            Tag = entity.Tag != null ? new TagUOWMapper().Map(entity.Tag) : null,
        };
    }

    public Domain.UserTag? Map(UserTag? entity)
    {
        if (entity == null) return null;
        
        return new Domain.UserTag()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            User = entity.User != null ? new AppUserUOWMapper().Map(entity.User) : null,
            TagId = entity.TagId,
            Tag = entity.Tag != null ? new TagUOWMapper().Map(entity.Tag) : null,
        };
    }
}