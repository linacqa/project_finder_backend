using Base.Contracts;
using DAL.DTO;
using DAL.DTO.Identity;

namespace DAL.EF.Mappers;

public class ProjectFolderUOWMapper : IMapper<DAL.DTO.ProjectFolder, Domain.ProjectFolder>
{
    public ProjectFolder? Map(Domain.ProjectFolder? entity)
    {
        if (entity == null) return null;
        
        return new ProjectFolder()
        {
            Id = entity.Id,
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
            FolderId = entity.FolderId,
            Folder = entity.Folder != null ? new Folder()
            {
                Id = entity.Folder.Id,
                Name = entity.Folder.Name,
            } : null,
        };
    }

    public Domain.ProjectFolder? Map(ProjectFolder? entity)
    {
        if (entity == null) return null;
        
        return new Domain.ProjectFolder()
        {
            Id = entity.Id,
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
            FolderId = entity.FolderId,
            Folder = entity.Folder != null ? new Domain.Folder()
            {
                Id = entity.Folder.Id,
                Name = entity.Folder.Name,
            } : null,
        };
    }
}