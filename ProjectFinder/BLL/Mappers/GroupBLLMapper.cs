using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class GroupBLLMapper : IMapper<BLL.DTO.Group, DAL.DTO.Group>
{
    public Group? Map(DAL.DTO.Group? entity)
    {
        if (entity == null) return null;
        
        var result = new Group()
        {
            Id = entity.Id,
            Name = entity.Name,
            IsAzureAdGroup = entity.IsAzureAdGroup,
            CreatorId = entity.CreatorId,
        };
        
        return result;
    }

    public DAL.DTO.Group? Map(Group? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.Group()
        {
            Id = entity.Id,
            Name = entity.Name,
            IsAzureAdGroup = entity.IsAzureAdGroup,
            CreatorId = entity.CreatorId,
        };
        
        return result;
    }
}