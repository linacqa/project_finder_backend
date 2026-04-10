using Base.Contracts;
using BLL.DTO;
using BLL.DTO.Identity;

namespace BLL.Mappers;

public class ApplicationBLLMapper : IMapper<BLL.DTO.Application, DAL.DTO.Application>
{
    public Application? Map(DAL.DTO.Application? entity)
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
                Creator = entity.Group.Creator == null ? null : new AppUser()
                {
                    Id = entity.Group.Creator.Id,
                    FirstName = entity.Group.Creator.FirstName,
                    LastName = entity.Group.Creator.LastName,
                    Email = entity.Group.Creator.Email,
                },
            },
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new Project()
            {
                Id = entity.Project.Id,
                TitleInEstonian = entity.Project.TitleInEstonian,
                TitleInEnglish = entity.Project.TitleInEnglish,
            } : null,
            UserId = entity.UserId,
            User = entity.User == null ? null : new AppUser()
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

    public DAL.DTO.Application? Map(Application? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.Application()
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
}
