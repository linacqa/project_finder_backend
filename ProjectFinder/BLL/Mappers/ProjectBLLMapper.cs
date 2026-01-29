using Base.Contracts;
using BLL.DTO;

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
            ExternalSupervisor = entity.ExternalSupervisor,
            MinStudents = entity.MinStudents,
            MaxStudents = entity.MaxStudents,
            ProjectType = entity.ProjectType,
            ProjectStatus = entity.ProjectStatus,
            Deadline = entity.Deadline,
            AttachmentsPaths = entity.AttachmentsPaths,
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
            ExternalSupervisor = entity.ExternalSupervisor,
            MinStudents = entity.MinStudents,
            MaxStudents = entity.MaxStudents,
            ProjectType = entity.ProjectType,
            ProjectStatus = entity.ProjectStatus,
            Deadline = entity.Deadline,
            AttachmentsPaths = entity.AttachmentsPaths,
        };
        
        return result;
    }
}