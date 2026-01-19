using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class ApplicationBLLMapper : IMapper<BLL.DTO.Application, DAL.DTO.Application>
{
    public Application? Map(DAL.DTO.Application? entity)
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

    public DAL.DTO.Application? Map(Application? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.Application()
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
}
