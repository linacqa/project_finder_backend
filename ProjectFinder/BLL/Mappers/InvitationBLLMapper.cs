using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class InvitationBLLMapper : IMapper<BLL.DTO.Invitation, DAL.DTO.Invitation>
{
    public Invitation? Map(DAL.DTO.Invitation? entity)
    {
        if (entity == null) return null;
        
        var result = new Invitation()
        {
            Id = entity.Id,
            GroupId = entity.GroupId,
            ProjectId = entity.ProjectId,
            FromUserId = entity.FromUserId,
            ToUserId = entity.ToUserId,
            AcceptedAt = entity.AcceptedAt,
            DeclinedAt = entity.DeclinedAt,
        };
        
        return result;
    }

    public DAL.DTO.Invitation? Map(Invitation? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.Invitation()
        {
            Id = entity.Id,
            GroupId = entity.GroupId,
            ProjectId = entity.ProjectId,
            FromUserId = entity.FromUserId,
            ToUserId = entity.ToUserId,
            AcceptedAt = entity.AcceptedAt,
            DeclinedAt = entity.DeclinedAt,
        };
        
        return result;
    }
}