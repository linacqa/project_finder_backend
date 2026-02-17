using Base.Contracts;
using DAL.DTO;

namespace DAL.EF.Mappers;

public class UserProjectRoleUOWMapper : IMapper<DAL.DTO.UserProjectRole, Domain.UserProjectRole>
{
    public UserProjectRole? Map(Domain.UserProjectRole? entity)
    {
        if (entity == null) return null;
        
        return new UserProjectRole()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }

    public Domain.UserProjectRole? Map(UserProjectRole? entity)
    {
        if (entity == null) return null;
        
        return new Domain.UserProjectRole()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }
}