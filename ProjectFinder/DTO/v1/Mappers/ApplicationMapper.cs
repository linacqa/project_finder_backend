using Base.Contracts;

namespace DTO.v1.Mappers;

public class ApplicationMapper : IMapper<DTO.v1.Application, BLL.DTO.Application>
{
    public Application? Map(BLL.DTO.Application? entity)
    {
        if (entity == null) return null;
        
        var result = new Application()
        {
            Id = entity.Id,
            GroupId = entity.GroupId,
            Group = entity.Group == null ? null : new Group()
            {
                Id = entity.Group.Id,
                Name = entity.Group.Name,
                IsAzureAdGroup = entity.Group.IsAzureAdGroup,
                CreatorId = entity.Group.CreatorId,
                Creator = entity.Group.Creator == null ? null : new Identity.UserInfo()
                {
                    Id = entity.Group.Creator.Id,
                    FirstName = entity.Group.Creator.FirstName,
                    LastName = entity.Group.Creator.LastName,
                    Email = entity.Group.Creator.Email,
                },
            },
            ProjectId = entity.ProjectId,
            UserId = entity.UserId,
            User = entity.User == null ? null : new Identity.UserInfo()
            {
                Id = entity.User.Id,
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName,
                Email = entity.User.Email,
            },
            AcceptedAt = entity.AcceptedAt,
            DeclinedAt = entity.DeclinedAt,
        };
        
        return result;
    }

    public BLL.DTO.Application? Map(Application? entity)
    {
        if (entity == null) return null;
        
        var result = new BLL.DTO.Application()
        {
            Id = entity.Id,
            GroupId = entity.GroupId,
            ProjectId = entity.ProjectId,
            UserId = entity.UserId,
            AcceptedAt = entity.AcceptedAt,
            DeclinedAt = entity.DeclinedAt,
        };
        
        return result;
    }

    public BLL.DTO.Application Map(ApplicationCreate entity)
    {
        var result = new BLL.DTO.Application()
        {
            Id = Guid.NewGuid(),
            GroupId = entity.GroupId,
            ProjectId = entity.ProjectId,
            // UserId = entity.UserId,
            AcceptedAt = null,
            DeclinedAt = null,
        };
        
        return result;
    }
}
