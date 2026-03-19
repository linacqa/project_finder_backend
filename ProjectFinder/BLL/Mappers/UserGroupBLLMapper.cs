using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class UserGroupBLLMapper : IMapper<BLL.DTO.UserGroup, DAL.DTO.UserGroup>
{
    public UserGroup? Map(DAL.DTO.UserGroup? entity)
    {
        if (entity == null) return null;
        
        var result = new UserGroup()
        {
            Id = entity.Id,
            GroupId = entity.GroupId,
            UserId = entity.UserId,
            Role = entity.Role,
        };
        
        return result;
    }

    public DAL.DTO.UserGroup? Map(UserGroup? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.UserGroup()
        {
            Id = entity.Id,
            GroupId = entity.GroupId,
            UserId = entity.UserId,
            Role = entity.Role,
        };
        
        return result;
    }
}