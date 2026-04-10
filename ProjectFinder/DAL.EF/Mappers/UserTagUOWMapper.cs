using Base.Contracts;
using DAL.DTO;
using DAL.DTO.Identity;
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
            User = entity.User != null ? new AppUser()
            {
                Id = entity.User.Id,
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName,
                AzureObjectId = entity.User.AzureObjectId,
                AuthType = entity.User.AuthType,
            } : null,
            TagId = entity.TagId,
            Tag = entity.Tag != null ? new Tag()
            {
                Id = entity.Tag.Id,
                Name = entity.Tag.Name,
            } : null,
        };
    }

    public Domain.UserTag? Map(UserTag? entity)
    {
        if (entity == null) return null;
        
        return new Domain.UserTag()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            TagId = entity.TagId,
        };
    }
}