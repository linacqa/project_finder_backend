using Base.Contracts;
using BLL.DTO;
using BLL.DTO.Identity;

namespace BLL.Mappers;

public class ProjectBLLMapper : IMapper<BLL.DTO.Project, DAL.DTO.Project>
{
    public Project? Map(DAL.DTO.Project? entity)
    {
        if (entity == null) return null;
        
        var result = new Project()
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
            ProjectTags = entity.ProjectTags?.Select(pt => new ProjectTag()
            {
                Id = pt.Id,
                ProjectId = pt.ProjectId,
                TagId = pt.TagId,
                Tag = pt.Tag != null ? new Tag()
                {
                    Id = pt.Tag.Id,
                    Name = pt.Tag.Name,
                } : null,
            }).ToList(),
            UserProjects = entity.UserProjects?.Select(up => new UserProject()
            {
                Id = up.Id,
                UserId = up.UserId,
                ProjectId = up.ProjectId,
                User = up.User != null ? new AppUser()
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

    public DAL.DTO.Project? Map(Project? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.Project()
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
            ProjectStatusId = entity.ProjectStatusId,
            Deadline = entity.Deadline,
            AttachmentsPaths = entity.AttachmentsPaths,
        };
        
        return result;
    }
}