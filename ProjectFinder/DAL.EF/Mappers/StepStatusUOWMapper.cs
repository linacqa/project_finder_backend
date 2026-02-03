using Base.Contracts;
using DAL.DTO;

namespace DAL.EF.Mappers;

public class StepStatusUOWMapper : IMapper<DAL.DTO.StepStatus, Domain.StepStatus>
{
    public StepStatus? Map(Domain.StepStatus? entity)
    {
        if (entity == null) return null;
        
        return new StepStatus()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }

    public Domain.StepStatus? Map(StepStatus? entity)
    {
        if (entity == null) return null;
        
        return new Domain.StepStatus()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }
}