using Base.Contracts;

namespace DTO.v1.Mappers;

public class StepStatusMapper : IMapper<DTO.v1.StepStatus, BLL.DTO.StepStatus>
{
    public StepStatus? Map(BLL.DTO.StepStatus? entity)
    {
        if (entity == null) return null;
        
        var result = new StepStatus()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }

    public BLL.DTO.StepStatus? Map(StepStatus? entity)
    {
        if (entity == null) return null;
        
        var result = new BLL.DTO.StepStatus()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }

    // public BLL.DTO.StepStatus Map(StepStatusCreate entity)
    // {
    //     var result = new BLL.DTO.StepStatus()
    //     {
    //         Id = Guid.NewGuid(),
    //         Name = entity.Name,
    //     };
    //     
    //     return result;
    // }
}