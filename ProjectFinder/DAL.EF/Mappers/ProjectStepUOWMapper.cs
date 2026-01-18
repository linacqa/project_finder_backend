using Base.Contracts;
using DAL.DTO;

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
            Project = entity.Project != null ? new ProjectUOWMapper().Map(entity.Project) : null,
            StepId = entity.StepId,
            Step = entity.Step != null ? new StepUOWMapper().Map(entity.Step) : null,
            StepStatus = entity.StepStatus,
        };
    }

    public Domain.ProjectStep? Map(ProjectStep? entity)
    {
        if (entity == null) return null;
        
        return new Domain.ProjectStep()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new ProjectUOWMapper().Map(entity.Project) : null,
            StepId = entity.StepId,
            Step = entity.Step != null ? new StepUOWMapper().Map(entity.Step) : null,
            StepStatus = entity.StepStatus,
        };
    }
}