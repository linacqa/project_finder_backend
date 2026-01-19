using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class UserTagBLLMapper : IMapper<BLL.DTO.UserTag, DAL.DTO.UserTag>
{
    public UserTag? Map(DAL.DTO.UserTag? entity)
    {
        if (entity == null) return null;
        
        var result = new UserTag()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            TagId = entity.TagId,
        };
        
        return result;
    }

    public DAL.DTO.UserTag? Map(UserTag? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.UserTag()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            TagId = entity.TagId,
        };
        
        return result;
    }
}