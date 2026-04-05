using Base.Contracts;
using DTO.v1.Identity;

namespace DTO.v1.Mappers;

public class InvitationMapper : IMapper<DTO.v1.Invitation, BLL.DTO.Invitation>
{
    public Invitation? Map(BLL.DTO.Invitation? entity)
    {
        if (entity == null) return null;
        
        var result = new Invitation()
        {
            Id = entity.Id,
            GroupId = entity.GroupId,
            Group = entity.Group != null ? new Group()
            {
                Id = entity.Group.Id,
                Name = entity.Group.Name,
                IsAzureAdGroup = entity.Group.IsAzureAdGroup,
                CreatorId = entity.Group.CreatorId,
                Creator = entity.Group.Creator != null ? new UserInfo()
                {
                    Id = entity.Group.Creator.Id,
                    FirstName = entity.Group.Creator.FirstName,
                    LastName = entity.Group.Creator.LastName,
                    Email = entity.Group.Creator.Email,
                } : null,
                Users = entity.Group.UserGroups?.Select(ug => new UserGroup()
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
            } : null,
            FromUserId = entity.FromUserId,
            FromUser = entity.FromUser != null ? new StudentInfo()
            {
                Id = entity.FromUser.Id,
                FirstName = entity.FromUser.FirstName,
                LastName = entity.FromUser.LastName,
                Email = entity.FromUser.Email,
            }  : null,
            ToUserId = entity.ToUserId,
            ToUser = entity.ToUser != null ? new StudentInfo()
            {
                Id = entity.ToUser.Id,
                FirstName = entity.ToUser.FirstName,
                LastName = entity.ToUser.LastName,
                Email = entity.ToUser.Email,
            }  : null,
            Role = entity.Role,
            AcceptedAt = entity.AcceptedAt,
            DeclinedAt = entity.DeclinedAt,
        };
        
        return result;
    }

    public BLL.DTO.Invitation? Map(Invitation? entity)
    {
        if (entity == null) return null;
        
        var result = new BLL.DTO.Invitation()
        {
            Id = entity.Id,
            GroupId = entity.GroupId,
            FromUserId = entity.FromUserId,
            ToUserId = entity.ToUserId,
            Role = entity.Role,
        };
        
        return result;
    }

    public BLL.DTO.Invitation Map(InvitationCreate entity)
    {
        var result = new BLL.DTO.Invitation()
        {
            Id = Guid.NewGuid(),
            GroupId = entity.GroupId,
            ToUserId = entity.ToUserId,
            Role = entity.Role,
        };
        
        return result;
    }
}