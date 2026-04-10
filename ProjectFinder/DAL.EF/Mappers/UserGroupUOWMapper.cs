using Base.Contracts;
using DAL.DTO;
using DAL.DTO.Identity;
using DAL.EF.Mappers.Identity;

namespace DAL.EF.Mappers;

public class UserGroupUOWMapper : IMapper<DAL.DTO.UserGroup, Domain.UserGroup>
{
    public UserGroup? Map(Domain.UserGroup? entity)
    {
        if (entity == null) return null;
        
        return new UserGroup()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            User = entity.User != null ? new AppUser()
            {
                Id = entity.User.Id,
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName,
                AzureObjectId = entity.User.AzureObjectId,
                AuthType = entity.User.AuthType,
            } : null,
            GroupId = entity.GroupId,
            Group = entity.Group != null ? new Group()
            {
                Id = entity.Group.Id,
                Name = entity.Group.Name,
                IsAzureAdGroup =  entity.Group.IsAzureAdGroup,
                CreatorId = entity.Group.UserId,
                Creator = entity.Group.User != null ? new AppUser()
                {
                    Id = entity.Group.User.Id,
                    FirstName = entity.Group.User.FirstName,
                    LastName = entity.Group.User.LastName,
                    AzureObjectId = entity.Group.User.AzureObjectId,
                    AuthType = entity.Group.User.AuthType,
                } : null,
            } : null,
            Role = entity.Role,
        };
    }

    public Domain.UserGroup? Map(UserGroup? entity)
    {
        if (entity == null) return null;
        
        return new Domain.UserGroup()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            GroupId = entity.GroupId,
            Role = entity.Role,
        };
    }
}