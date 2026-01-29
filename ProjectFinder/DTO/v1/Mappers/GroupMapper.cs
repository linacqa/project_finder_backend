using Base.Contracts;

namespace DTO.v1.Mappers;

public class GroupMapper : IMapper<DTO.v1.Group, BLL.DTO.Group>
{
    public Group? Map(BLL.DTO.Group? entity)
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

    public BLL.DTO.Group? Map(Group? entity)
    {
        if (entity == null) return null;
        
        var result = new BLL.DTO.Group()
        {
            Id = entity.Id,
            Name = entity.Name,
            IsAzureAdGroup = entity.IsAzureAdGroup,
            CreatorId = entity.CreatorId,
        };
        
        return result;
    }

    public BLL.DTO.Group Map(GroupCreateUpdate entity)
    {
        var result = new BLL.DTO.Group()
        {
            Id = Guid.NewGuid(),
            Name = entity.Name,
            IsAzureAdGroup = false,
        };
        
        return result;
    }
}