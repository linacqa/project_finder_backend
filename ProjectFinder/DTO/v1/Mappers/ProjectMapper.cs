using Base.Contracts;
using DTO.v1.Identity;

namespace DTO.v1.Mappers;

public class ProjectMapper : IMapper<DTO.v1.Project, BLL.DTO.Project>
{
    public Project? Map(BLL.DTO.Project? entity)
    {
        if (entity == null) return null;
        
        var result = new Project()
        {
            Id = entity.Id,
            TitleInEstonian = entity.TitleInEstonian,
            TitleInEnglish = entity.TitleInEnglish,
            Description = entity.Description,
            Client = entity.Client,
            Supervisor = entity.PrimarySupervisor,
            ExternalSupervisor = entity.ExternalSupervisor,
            MinStudents = entity.MinStudents,
            MaxStudents = entity.MaxStudents,
            ProjectTypeId = entity.ProjectTypeId,
            ProjectType = entity.ProjectType != null ? new ProjectType()
            {
                Id = entity.ProjectType.Id,
                Name = entity.ProjectType.Name,
            } : null,
            ProjectStatusId = entity.ProjectStatusId,
            ProjectStatus = entity.ProjectStatus != null ? new ProjectStatus()
            {
                Id = entity.ProjectStatus.Id,
                Name = entity.ProjectStatus.Name,
            } : null,
            Deadline = entity.Deadline,
            AttachmentsPaths = entity.AttachmentsPaths,
            Tags = entity.ProjectTags?.Select(pt => new Tag()
            {
                Id = pt.TagId,
                Name = pt.Tag != null ? pt.Tag.Name : "",
            }).ToList(),
            Users = entity.UserProjects?.Select(up => new UserProject()
            {
                Id = up.Id,
                UserId = up.UserId,
                ProjectId = up.ProjectId,
                User = up.User != null ? new UserInfo()
                {
                    Id = up.User.Id,
                    FirstName = up.User.FirstName,
                    LastName = up.User.LastName,
                    Email = up.User.Email,
                } : null,
                UserProjectRoleId = up.UserProjectRoleId,
                UserProjectRole = up.UserProjectRole != null ? new UserProjectRole()
                {
                    Id = up.UserProjectRole.Id,
                    Name = up.UserProjectRole.Name,
                } : null,
            }).ToList(),
        };
        
        return result;
    }

    public BLL.DTO.Project? Map(Project? entity)
    {
        if (entity == null) return null;
        
        var result = new BLL.DTO.Project()
        {
            Id = entity.Id,
            TitleInEstonian = entity.TitleInEstonian,
            TitleInEnglish = entity.TitleInEnglish,
            Description = entity.Description,
            Client = entity.Client,
            PrimarySupervisor = entity.Supervisor,
            ExternalSupervisor = entity.ExternalSupervisor,
            MinStudents = entity.MinStudents,
            MaxStudents = entity.MaxStudents,
            ProjectTypeId = entity.ProjectTypeId,
            ProjectStatusId = entity.ProjectStatusId,
            Deadline = entity.Deadline,
            AttachmentsPaths = entity.AttachmentsPaths,
        };
        
        return result;
    }

    public BLL.DTO.Project Map(ProjectCreate entity)
    {
        var result = new BLL.DTO.Project()
        {
            Id = Guid.NewGuid(),
            TitleInEstonian = entity.TitleInEstonian,
            TitleInEnglish = entity.TitleInEnglish,
            Description = entity.Description,
            Client = entity.Client,
            ExternalSupervisor = entity.ExternalSupervisor,
            MinStudents = entity.MinStudents,
            MaxStudents = entity.MaxStudents,
            ProjectTypeId = entity.ProjectTypeId,
            ProjectStatusId = entity.ProjectStatusId,
            Deadline = entity.Deadline,
            AttachmentsPaths = new List<string>(),
            FolderIds = entity.FolderIds,
            TagIds = entity.TagIds,
            StepIds = entity.StepIds,
            AuthorId = entity.AuthorId,
            ExternalSupervisorId = entity.ExternalSupervisorId,
            PrimarySupervisorId = entity.PrimarySupervisorId,
            PrimarySupervisor = entity.PrimarySupervisor,
        };
        
        return result;
    }
}