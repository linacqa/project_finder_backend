using Base.Contracts;
using DAL.DTO;
using DAL.DTO.Identity;
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
            Creator = entity.User != null ? new AppUser()
            {
                Id = entity.User.Id,
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName,
                Email = entity.User.Email,
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
            User = entity.Creator != null ? new Domain.Identity.AppUser()
            {
                Id = entity.Creator.Id,
                FirstName = entity.Creator.FirstName,
                LastName = entity.Creator.LastName,
                AzureObjectId = entity.Creator.AzureObjectId,
                AuthType = entity.Creator.AuthType,
            } : null,
        };
    }
}