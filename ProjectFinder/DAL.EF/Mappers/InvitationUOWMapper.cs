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
                    AzureObjectId = entity.Group.User.AzureObjectId,
                    AuthType = entity.Group.User.AuthType,
                } : null,
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
                ProjectType = entity.Project.ProjectType,
                ProjectStatus = entity.Project.ProjectStatus,
                Deadline = entity.Project.Deadline,
                AttachmentsPaths = entity.Project.AttachmentsPaths,
                CreatorId = entity.Project.UserId,
                Creator = entity.Project.User != null ? new AppUser()
                {
                    Id = entity.Project.User.Id,
                    FirstName = entity.Project.User.FirstName,
                    LastName = entity.Project.User.LastName,
                    AzureObjectId = entity.Project.User.AzureObjectId,
                    AuthType = entity.Project.User.AuthType,
                } : null,
            } : null,
            ToUserId = entity.UserId,
            ToUser = entity.User != null ? new AppUser()
            {
                Id = entity.User.Id,
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName,
                AzureObjectId = entity.User.AzureObjectId,
                AuthType = entity.User.AuthType,
            } : null,
            FromUserId = entity.FromUserId,
            FromUser = entity.FromUser != null ? new AppUser()
            {
                Id = entity.FromUser.Id,
                FirstName = entity.FromUser.FirstName,
                LastName = entity.FromUser.LastName,
                AzureObjectId = entity.FromUser.AzureObjectId,
                AuthType = entity.FromUser.AuthType,
            } : null,
            AcceptedAt = entity.AcceptedAt,
            DeclinedAt = entity.DeclinedAt,
        };
    }

    public Domain.Invitation? Map(Invitation? entity)
    {
        if (entity == null) return null;
        
        return new Domain.Invitation()
        {
            Id = entity.Id,
            GroupId = entity.GroupId,
            Group = entity.Group != null ? new Domain.Group()
            {
                Id = entity.Group.Id,
                Name = entity.Group.Name,
                IsAzureAdGroup =  entity.Group.IsAzureAdGroup,
                UserId = entity.Group.CreatorId,
                User = entity.Group.Creator != null ? new Domain.Identity.AppUser()
                {
                    Id = entity.Group.Creator.Id,
                    FirstName = entity.Group.Creator.FirstName,
                    LastName = entity.Group.Creator.LastName,
                    AzureObjectId = entity.Group.Creator.AzureObjectId,
                    AuthType = entity.Group.Creator.AuthType,
                } : null,
            } : null,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new Domain.Project()
            {
                Id = entity.Project.Id,
                TitleInEstonian = entity.Project.TitleInEstonian,
                TitleInEnglish = entity.Project.TitleInEnglish,
                Description = entity.Project.Description,
                Client = entity.Project.Client,
                ExternalSupervisor = entity.Project.ExternalSupervisor,
                MinStudents = entity.Project.MinStudents,
                MaxStudents = entity.Project.MaxStudents,
                ProjectType = entity.Project.ProjectType,
                ProjectStatus = entity.Project.ProjectStatus,
                Deadline = entity.Project.Deadline,
                AttachmentsPaths = entity.Project.AttachmentsPaths,
                UserId = entity.Project.CreatorId,
                User = entity.Project.Creator != null ? new Domain.Identity.AppUser()
                {
                    Id = entity.Project.Creator.Id,
                    FirstName = entity.Project.Creator.FirstName,
                    LastName = entity.Project.Creator.LastName,
                    AzureObjectId = entity.Project.Creator.AzureObjectId,
                    AuthType = entity.Project.Creator.AuthType,
                } : null,
            } : null,
            UserId = entity.ToUserId,
            User = entity.ToUser != null ? new Domain.Identity.AppUser()
            {
                Id = entity.ToUser.Id,
                FirstName = entity.ToUser.FirstName,
                LastName = entity.ToUser.LastName,
                AzureObjectId = entity.ToUser.AzureObjectId,
                AuthType = entity.ToUser.AuthType,
            } : null,
            FromUserId = entity.FromUserId,
            FromUser = entity.FromUser != null ? new Domain.Identity.AppUser()
            {
                Id = entity.FromUser.Id,
                FirstName = entity.FromUser.FirstName,
                LastName = entity.FromUser.LastName,
                AzureObjectId = entity.FromUser.AzureObjectId,
                AuthType = entity.FromUser.AuthType,
            } : null,
            AcceptedAt = entity.AcceptedAt,
            DeclinedAt = entity.DeclinedAt,
        };
    }
}