using Base.Contracts;
using DAL.DTO;
using DAL.DTO.Identity;

namespace DAL.EF.Mappers;

public class GroupProjectUOWMapper : IMapper<DAL.DTO.GroupProject, Domain.GroupProject>
{
    public GroupProject? Map(Domain.GroupProject? entity)
    {
        if (entity == null) return null;
        
        return new GroupProject()
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
            } : null,
        };
    }

    public Domain.GroupProject? Map(GroupProject? entity)
    {
        if (entity == null) return null;
        
        return new Domain.GroupProject()
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
            } : null,
        };
    }
}