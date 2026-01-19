using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class StepBLLMapper : IMapper<BLL.DTO.Step, DAL.DTO.Step>
{
    public Step? Map(DAL.DTO.Step? entity)
    {
        if (entity == null) return null;
        
        var result = new Step()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }

    public DAL.DTO.Step? Map(Step? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.Step()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }
}