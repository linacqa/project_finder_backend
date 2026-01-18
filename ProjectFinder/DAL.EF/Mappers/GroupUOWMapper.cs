using Base.Contracts;
using DAL.DTO;

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
        };
    }
}