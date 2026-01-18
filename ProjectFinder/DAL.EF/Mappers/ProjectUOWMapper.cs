using Base.Contracts;
using DAL.DTO;
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
            ExternalSupervisor = entity.ExternalSupervisor,
            MinStudents = entity.MinStudents,
            MaxStudents = entity.MaxStudents,
            ProjectType = entity.ProjectType,
            ProjectStatus = entity.ProjectStatus,
            Deadline = entity.Deadline,
            AttachmentsPaths = entity.AttachmentsPaths,
            CreatorId = entity.UserId,
            Creator = entity.User != null ? new AppUserUOWMapper().Map(entity.User) : null,
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
            ExternalSupervisor = entity.ExternalSupervisor,
            MinStudents = entity.MinStudents,
            MaxStudents = entity.MaxStudents,
            ProjectType = entity.ProjectType,
            ProjectStatus = entity.ProjectStatus,
            Deadline = entity.Deadline,
            AttachmentsPaths = entity.AttachmentsPaths,
            UserId = entity.CreatorId,
            User = entity.Creator != null ? new AppUserUOWMapper().Map(entity.Creator) : null,
        };
    }
}