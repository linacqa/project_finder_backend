using Base.Contracts;
using DAL.DTO;
using DAL.DTO.Identity;

namespace DAL.EF.Mappers;

public class ProjectStepUOWMapper : IMapper<DAL.DTO.ProjectStep, Domain.ProjectStep>
{
    public ProjectStep? Map(Domain.ProjectStep? entity)
    {
        if (entity == null) return null;
        
        return new ProjectStep()
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
            } : null,
            StepId = entity.StepId,
            Step = entity.Step != null ? new Step()
            {
                Id = entity.Step.Id,
                Name = entity.Step.Name,
            } : null,
            StepStatus = entity.StepStatus,
            Order = entity.Order,
        };
    }

    public Domain.ProjectStep? Map(ProjectStep? entity)
    {
        if (entity == null) return null;
        
        return new Domain.ProjectStep()
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
            } : null,
            StepId = entity.StepId,
            Step = entity.Step != null ? new Domain.Step()
            {
                Id = entity.Step.Id,
                Name = entity.Step.Name,
            } : null,
            StepStatus = entity.StepStatus,
            Order = entity.Order,
        };
    }
}