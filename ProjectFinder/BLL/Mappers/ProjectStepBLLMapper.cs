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
            StepStatus = entity.StepStatus,
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
            StepStatus = entity.StepStatus,
        };
        
        return result;
    }
}