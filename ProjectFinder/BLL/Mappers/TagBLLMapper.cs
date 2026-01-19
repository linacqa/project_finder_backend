using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class TagBLLMapper : IMapper<BLL.DTO.Tag, DAL.DTO.Tag>
{
    public Tag? Map(DAL.DTO.Tag? entity)
    {
        if (entity == null) return null;
        
        var result = new Tag()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }

    public DAL.DTO.Tag? Map(Tag? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.Tag()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }
}