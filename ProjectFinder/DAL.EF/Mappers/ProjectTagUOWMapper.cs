using Base.Contracts;
using DAL.DTO;
using DAL.DTO.Identity;

namespace DAL.EF.Mappers;

public class ProjectTagUOWMapper : IMapper<DAL.DTO.ProjectTag, Domain.ProjectTag>
{
    public ProjectTag? Map(Domain.ProjectTag? entity)
    {
        if (entity == null) return null;
        
        return new ProjectTag()
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
            TagId = entity.TagId,
            Tag = entity.Tag != null ? new Tag()
            {
                Id = entity.Tag.Id,
                Name = entity.Tag.Name,
            } : null
        };
    }

    public Domain.ProjectTag? Map(ProjectTag? entity)
    {
        if (entity == null) return null;
        
        return new Domain.ProjectTag()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            TagId = entity.TagId,
        };
    }
}