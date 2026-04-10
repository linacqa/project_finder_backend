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
            StepId = entity.StepId,
            Step = entity.Step != null ? new Step()
            {
                Id = entity.Step.Id,
                Name = entity.Step.Name,
            } : null,
            StepStatusId = entity.StepStatusId,
            StepStatus = entity.StepStatus != null ? new StepStatus()
            {
                Id = entity.StepStatus.Id,
                Name = entity.StepStatus.Name,
            } : null,
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
            StepId = entity.StepId,
            StepStatusId = entity.StepStatusId,
            Order = entity.Order,
        };
    }
}