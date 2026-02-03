using Base.Contracts;

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
        };
        
        return result;
    }
}