using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class ProjectStepBLLMapper : IMapper<BLL.DTO.ProjectStep, DAL.DTO.ProjectStep>
{
    public ProjectStep? Map(DAL.DTO.ProjectStep? entity)
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

    public DAL.DTO.ProjectStep? Map(ProjectStep? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.ProjectStep()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            StepId = entity.StepId,
            StepStatusId = entity.StepStatusId,
            Order = entity.Order,
        };
        
        return result;
    }
}