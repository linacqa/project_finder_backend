using Base.Contracts;
using DAL.DTO;
using DAL.DTO.Identity;
using DAL.EF.Mappers.Identity;

namespace DAL.EF.Mappers;

public class InvitationUOWMapper : IMapper<DAL.DTO.Invitation, Domain.Invitation>
{
    public Invitation? Map(Domain.Invitation? entity)
    {
        if (entity == null) return null;
        
        return new Invitation()
        {
            Id = entity.Id,
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
                    Email = entity.Group.User.Email,
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
            // ProjectId = entity.ProjectId,
            // Project = entity.Project != null ? new Project()
            // {
            //     Id = entity.Project.Id,
            //     TitleInEstonian = entity.Project.TitleInEstonian,
            //     TitleInEnglish = entity.Project.TitleInEnglish,
            //     Description = entity.Project.Description,
            //     Client = entity.Project.Client,
            //     ExternalSupervisor = entity.Project.ExternalSupervisor,
            //     MinStudents = entity.Project.MinStudents,
            //     MaxStudents = entity.Project.MaxStudents,
            //     ProjectTypeId = entity.Project.ProjectTypeId,
            //     ProjectType = entity.Project.ProjectType != null ? new ProjectType()
            //     {
            //         Id = entity.Project.ProjectType.Id,
            //         Name = entity.Project.ProjectType.Name,
            //     } : null,
            //     ProjectStatusId = entity.Project.ProjectStatusId,
            //     ProjectStatus = entity.Project.ProjectStatus != null ? new ProjectStatus()
            //     {
            //         Id = entity.Project.ProjectStatus.Id,
            //         Name = entity.Project.ProjectStatus.Name,
            //     } : null,
            //     Deadline = entity.Project.Deadline,
            //     AttachmentsPaths = entity.Project.AttachmentsPaths,
            // } : null,
            ToUserId = entity.ToUserId,
            ToUser = entity.ToUser != null ? new AppUser()
            {
                Id = entity.ToUser.Id,
                FirstName = entity.ToUser.FirstName,
                LastName = entity.ToUser.LastName,
                Email = entity.ToUser.Email,
                AzureObjectId = entity.ToUser.AzureObjectId,
                AuthType = entity.ToUser.AuthType,
            } : null,
            FromUserId = entity.UserId,
            FromUser = entity.User != null ? new AppUser()
            {
                Id = entity.User.Id,
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName,
                Email = entity.User.Email,
                AzureObjectId = entity.User.AzureObjectId,
                AuthType = entity.User.AuthType,
            } : null,
            AcceptedAt = entity.AcceptedAt,
            DeclinedAt = entity.DeclinedAt,
            Role = entity.Role,
        };
    }

    public Domain.Invitation? Map(Invitation? entity)
    {
        if (entity == null) return null;
        
        return new Domain.Invitation()
        {
            Id = entity.Id,
            GroupId = entity.GroupId,
            // Group = entity.Group != null ? new Domain.Group()
            // {
            //     Id = entity.Group.Id,
            //     Name = entity.Group.Name,
            //     IsAzureAdGroup =  entity.Group.IsAzureAdGroup,
            //     UserId = entity.Group.CreatorId,
            //     User = entity.Group.Creator != null ? new Domain.Identity.AppUser()
            //     {
            //         Id = entity.Group.Creator.Id,
            //         FirstName = entity.Group.Creator.FirstName,
            //         LastName = entity.Group.Creator.LastName,
            //         AzureObjectId = entity.Group.Creator.AzureObjectId,
            //         AuthType = entity.Group.Creator.AuthType,
            //     } : null,
            // } : null,
            // ProjectId = entity.ProjectId,
            // Project = entity.Project != null ? new Domain.Project()
            // {
            //     Id = entity.Project.Id,
            //     TitleInEstonian = entity.Project.TitleInEstonian,
            //     TitleInEnglish = entity.Project.TitleInEnglish,
            //     Description = entity.Project.Description,
            //     Client = entity.Project.Client,
            //     ExternalSupervisor = entity.Project.ExternalSupervisor,
            //     MinStudents = entity.Project.MinStudents,
            //     MaxStudents = entity.Project.MaxStudents,
            //     ProjectTypeId = entity.Project.ProjectTypeId,
            //     ProjectType = entity.Project.ProjectType != null ? new Domain.ProjectType()
            //     {
            //         Id = entity.Project.ProjectType.Id,
            //         Name = entity.Project.ProjectType.Name,
            //     } : null,
            //     ProjectStatusId = entity.Project.ProjectStatusId,
            //     ProjectStatus = entity.Project.ProjectStatus != null ? new Domain.ProjectStatus()
            //     {
            //         Id = entity.Project.ProjectStatus.Id,
            //         Name = entity.Project.ProjectStatus.Name,
            //     } : null,
            //     Deadline = entity.Project.Deadline,
            //     AttachmentsPaths = entity.Project.AttachmentsPaths,
            // } : null,
            UserId = entity.FromUserId,
            // User = entity.FromUser != null ? new Domain.Identity.AppUser()
            // {
            //     Id = entity.FromUser.Id,
            //     FirstName = entity.FromUser.FirstName,
            //     LastName = entity.FromUser.LastName,
            //     AzureObjectId = entity.FromUser.AzureObjectId,
            //     AuthType = entity.FromUser.AuthType,
            // } : null,
            ToUserId = entity.ToUserId,
            // ToUser = entity.ToUser != null ? new Domain.Identity.AppUser()
            // {
            //     Id = entity.ToUser.Id,
            //     FirstName = entity.ToUser.FirstName,
            //     LastName = entity.ToUser.LastName,
            //     AzureObjectId = entity.ToUser.AzureObjectId,
            //     AuthType = entity.ToUser.AuthType,
            // } : null,
            AcceptedAt = entity.AcceptedAt,
            DeclinedAt = entity.DeclinedAt,
            Role = entity.Role,
        };
    }
}