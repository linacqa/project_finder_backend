using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class StepStatusBLLMapper : IMapper<BLL.DTO.StepStatus, DAL.DTO.StepStatus>
{
    public StepStatus? Map(DAL.DTO.StepStatus? entity)
    {
        if (entity == null) return null;
        
        var result = new StepStatus()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }

    public DAL.DTO.StepStatus? Map(StepStatus? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.StepStatus()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }
}