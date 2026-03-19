using Base.Contracts;
using DTO.v1.Identity;

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
                Creator = entity.Creator != null ? new UserInfo()
                {
                    Id = entity.Creator.Id,
                    FirstName = entity.Creator.FirstName,
                    LastName = entity.Creator.LastName,
                    Email = entity.Creator.Email,
                } : null,
                Users = entity.UserGroups?.Select(ug => new UserGroup()
                {
                    Id = ug.Id,
                    UserId = ug.UserId,
                    GroupId = ug.GroupId,
                    Role = ug.Role,
                    User = ug.User != null ? new UserInfo()
                    {
                        Id = ug.User.Id,
                        FirstName = ug.User.FirstName,
                        LastName = ug.User.LastName,
                        Email = ug.User.Email,
                    } : null,
                }).ToList(),
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
            CreatorRoleInGroup = entity.CreatorRoleInGroup,
        };
        
        return result;
    }
}