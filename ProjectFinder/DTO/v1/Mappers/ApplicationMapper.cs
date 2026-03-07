using Base.Contracts;

namespace DTO.v1.Mappers;

public class ApplicationMapper : IMapper<DTO.v1.Application, BLL.DTO.Application>
{
    public Application? Map(BLL.DTO.Application? entity)
    {
        if (entity == null) return null;
        
        var result = new Application()
        {
            Id = entity.Id,
            GroupId = entity.GroupId,
            ProjectId = entity.ProjectId,
            UserId = entity.UserId,
            AcceptedAt = entity.AcceptedAt,
            DeclinedAt = entity.DeclinedAt,
        };
        
        return result;
    }

    public BLL.DTO.Application? Map(Application? entity)
    {
        if (entity == null) return null;
        
        var result = new BLL.DTO.Application()
        {
            Id = entity.Id,
            GroupId = entity.GroupId,
            ProjectId = entity.ProjectId,
            UserId = entity.UserId,
            AcceptedAt = entity.AcceptedAt,
            DeclinedAt = entity.DeclinedAt,
        };
        
        return result;
    }

    public BLL.DTO.Application Map(ApplicationCreate entity)
    {
        var result = new BLL.DTO.Application()
        {
            Id = Guid.NewGuid(),
            GroupId = entity.GroupId,
            ProjectId = entity.ProjectId,
            // UserId = entity.UserId,
            AcceptedAt = null,
            DeclinedAt = null,
        };
        
        return result;
    }
}
