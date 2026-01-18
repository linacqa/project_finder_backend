using Base.Contracts;
using DAL.DTO;

namespace DAL.EF.Mappers;

public class StepUOWMapper : IMapper<DAL.DTO.Step, Domain.Step>
{
    public Step? Map(Domain.Step? entity)
    {
        if (entity == null) return null;

        return new Step()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }

    public Domain.Step? Map(Step? entity)
    {
        if (entity == null) return null;

        return new Domain.Step()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }
}