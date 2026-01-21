using Base.Contracts;

namespace DTO.v1.Mappers;

public class StepMapper : IMapper<DTO.v1.Step, BLL.DTO.Step>
{
    public Step? Map(BLL.DTO.Step? entity)
    {
        if (entity == null) return null;
        
        var result = new Step()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }

    public BLL.DTO.Step? Map(Step? entity)
    {
        if (entity == null) return null;
        
        var result = new BLL.DTO.Step()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }

    public BLL.DTO.Step Map(StepCreate entity)
    {
        var result = new BLL.DTO.Step()
        {
            Id = Guid.NewGuid(),
            Name = entity.Name,
        };
        
        return result;
    }
}