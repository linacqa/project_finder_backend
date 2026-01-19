using Base.Contracts;
using DAL.DTO;
using DAL.EF.Mappers.Identity;

namespace DAL.EF.Mappers;

public class GroupUOWMapper : IMapper<DAL.DTO.Group, Domain.Group>
{
    public Group? Map(Domain.Group? entity)
    {
        if (entity == null) return null;
        
        return new Group()
        {
            Id = entity.Id,
            Name = entity.Name,
            IsAzureAdGroup =  entity.IsAzureAdGroup,
            CreatorId = entity.UserId,
            Creator = entity.User != null ? new AppUserUOWMapper().Map(entity.User) : null,
        };
    }

    public Domain.Group? Map(Group? entity)
    {
        if (entity == null) return null;
        
        return new Domain.Group()
        {
            Id = entity.Id,
            Name = entity.Name,
            IsAzureAdGroup =  entity.IsAzureAdGroup,
            UserId = entity.CreatorId,
            User = entity.Creator != null ? new AppUserUOWMapper().Map(entity.Creator) : null,
        };
    }
}