using Base.Contracts;

namespace DTO.v1.Mappers;

public class TagMapper : IMapper<DTO.v1.Tag, BLL.DTO.Tag>
{
    public Tag? Map(BLL.DTO.Tag? entity)
    {
        if (entity == null) return null;
        
        var result = new Tag()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }

    public BLL.DTO.Tag? Map(Tag? entity)
    {
        if (entity == null) return null;
        
        var result = new BLL.DTO.Tag()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        
        return result;
    }

    public BLL.DTO.Tag Map(TagCreate entity)
    {
        var result = new BLL.DTO.Tag()
        {
            Id = Guid.NewGuid(),
            Name = entity.Name,
        };
        
        return result;
    }
}