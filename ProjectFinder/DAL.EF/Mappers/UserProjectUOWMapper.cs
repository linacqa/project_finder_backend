using Base.Contracts;
using DAL.DTO;
using DAL.DTO.Identity;
using DAL.EF.Mappers.Identity;

namespace DAL.EF.Mappers;

public class UserProjectUOWMapper : IMapper<DAL.DTO.UserProject, Domain.UserProject>
{
    public UserProject? Map(Domain.UserProject? entity)
    {
        if (entity == null) return null;
        
        return new UserProject()
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

    public Domain.UserProject? Map(UserProject? entity)
    {
        if (entity == null) return null;
        
        return new Domain.UserProject()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            User = entity.User != null ? new Domain.Identity.AppUser()
            {
                Id = entity.User.Id,
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName,
                AzureObjectId = entity.User.AzureObjectId,
                AuthType = entity.User.AuthType,
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