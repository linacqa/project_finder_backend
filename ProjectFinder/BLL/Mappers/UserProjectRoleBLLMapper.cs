using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class UserProjectRoleBLLMapper : IMapper<BLL.DTO.UserProjectRole, DAL.DTO.UserProjectRole>
{
    public UserProjectRole? Map(DAL.DTO.UserProjectRole? entity)
    {
        if (entity == null) return null;
        
        var result = new UserProjectRole()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }

    public DAL.DTO.UserProjectRole? Map(UserProjectRole? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.UserProjectRole()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }
}