using Base.Contracts;
using DAL.DTO;
using DAL.DTO.Identity;
using DAL.EF.Mappers.Identity;

namespace DAL.EF.Mappers;

public class ProjectUOWMapper : IMapper<DAL.DTO.Project, Domain.Project>
{
    public Project? Map(Domain.Project? entity)
    {
        if (entity == null) return null;

        return new Project()
        {
            Id = entity.Id,
            TitleInEstonian = entity.TitleInEstonian,
            TitleInEnglish = entity.TitleInEnglish,
            Description = entity.Description,
            Client = entity.Client,
            Supervisor = entity.Supervisor,
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
    }

    public Domain.Project? Map(Project? entity)
    {
        if (entity == null) return null;

        return new Domain.Project()
        {
            Id = entity.Id,
            TitleInEstonian = entity.TitleInEstonian,
            TitleInEnglish = entity.TitleInEnglish,
            Description = entity.Description,
            Client = entity.Client,
            Supervisor = entity.Supervisor,
            ExternalSupervisor = entity.ExternalSupervisor,
            MinStudents = entity.MinStudents,
            MaxStudents = entity.MaxStudents,
            ProjectTypeId = entity.ProjectTypeId,
            // ProjectType = entity.ProjectType != null ? new Domain.ProjectType()
            // {
            //     Id = entity.ProjectType.Id,
            //     Name = entity.ProjectType.Name,
            // } : null,
            ProjectStatusId = entity.ProjectStatusId,
            // ProjectStatus = entity.ProjectStatus != null ? new Domain.ProjectStatus()
            // {
            //     Id = entity.ProjectStatus.Id,
            //     Name = entity.ProjectStatus.Name,
            // } : null,
            Deadline = entity.Deadline,
            AttachmentsPaths = entity.AttachmentsPaths,
        };
    }
}