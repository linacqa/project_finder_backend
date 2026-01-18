using Base.Contracts;
using DAL.DTO;

namespace DAL.EF.Mappers;

public class TagUOWMapper : IMapper<DAL.DTO.Tag, Domain.Tag>
{
    public Tag? Map(Domain.Tag? entity)
    {
        if (entity == null) return null;
        
        return new Tag()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }

    public Domain.Tag? Map(Tag? entity)
    {
        if (entity == null) return null;
        
        return new Domain.Tag()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }
}