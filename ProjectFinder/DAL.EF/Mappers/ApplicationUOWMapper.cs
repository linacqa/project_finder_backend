using Base.Contracts;
using DAL.DTO;
using DAL.DTO.Identity;
using DAL.EF.Mappers.Identity;

namespace DAL.EF.Mappers;

public class ApplicationUOWMapper : IMapper<DAL.DTO.Application, Domain.Application>
{
    public Application? Map(Domain.Application? entity)
    {
        if (entity == null) return null;

        return new Application()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            User = entity.User != null ? new AppUser()
            {
                Id = entity.User.Id,
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName,
                Email = entity.User.Email,
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
                UserGroups = entity.Group.UserGroups?.Select(ug => new UserGroup()
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
            } : null,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new Project()
            {
                Id = entity.Project.Id,
                TitleInEstonian = entity.Project.TitleInEstonian,
                TitleInEnglish = entity.Project.TitleInEnglish,
                Description = entity.Project.Description,
                Client = entity.Project.Client,
                ExternalSupervisor = entity.Project.ExternalSupervisor,
                MinStudents = entity.Project.MinStudents,
                MaxStudents = entity.Project.MaxStudents,
                ProjectTypeId = entity.Project.ProjectTypeId,
                ProjectType = entity.Project.ProjectType != null ? new ProjectType()
                {
                    Id = entity.Project.ProjectType.Id,
                    Name = entity.Project.ProjectType.Name,
                } : null,
                ProjectStatusId = entity.Project.ProjectStatusId,
                ProjectStatus = entity.Project.ProjectStatus != null ? new ProjectStatus()
                {
                    Id = entity.Project.ProjectStatus.Id,
                    Name = entity.Project.ProjectStatus.Name,
                } : null,
                Deadline = entity.Project.Deadline,
                AttachmentsPaths = entity.Project.AttachmentsPaths,
            } : null,
            AcceptedAt = entity.AcceptedAt,
            DeclinedAt = entity.DeclinedAt,
        };
    }

    public Domain.Application? Map(Application? entity)
    {
        if (entity == null) return null;
        
        return new Domain.Application()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            GroupId = entity.GroupId,
            ProjectId = entity.ProjectId,
            AcceptedAt = entity.AcceptedAt,
            DeclinedAt = entity.DeclinedAt,
        };
    }
}