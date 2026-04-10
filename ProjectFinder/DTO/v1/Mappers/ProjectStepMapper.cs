using Base.Contracts;
using DTO.v1.Identity;

namespace DTO.v1.Mappers;

public class ProjectStepMapper : IMapper<DTO.v1.ProjectStep, BLL.DTO.ProjectStep>
{
    public ProjectStep? Map(BLL.DTO.ProjectStep? entity)
    {
        if (entity == null) return null;
        
        var result = new ProjectStep()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
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
        
        return result;
    }

    public BLL.DTO.ProjectStep? Map(ProjectStep? entity)
    {
        if (entity == null) return null;
        
        var result = new BLL.DTO.ProjectStep()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            StepId = entity.StepId,
            StepStatusId = entity.StepStatusId,
            Order = entity.Order,
        };
        
        return result;
    }

    public BLL.DTO.ProjectStep Map(ProjectStepCreateUpdate entity)
    {
        var result = new BLL.DTO.ProjectStep()
        {
            Id = Guid.NewGuid(),
            ProjectId = entity.ProjectId,
            StepId = entity.StepId,
            StepStatusId = entity.StepStatusId,
            Order = entity.Order,
        };
        
        return result;
    }
}