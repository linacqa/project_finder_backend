using Base.Contracts;
using BLL.DTO;
using BLL.DTO.Identity;

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
            Creator = entity.Creator != null ? new AppUser()
            {
                Id = entity.Creator.Id,
                FirstName = entity.Creator.FirstName,
                LastName = entity.Creator.LastName,
                Email = entity.Creator.Email,
            } : null,
            UserGroups = entity.UserGroups?.Select(ug => new UserGroup()
            {
                Id = ug.Id,
                UserId = ug.UserId,
                GroupId = ug.GroupId,
                Role = ug.Role,
                User = ug.User != null ? new AppUser()
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