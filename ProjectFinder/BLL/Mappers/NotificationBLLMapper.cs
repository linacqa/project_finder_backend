using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class NotificationBLLMapper : IMapper<BLL.DTO.Notification, DAL.DTO.Notification>
{
    public Notification? Map(DAL.DTO.Notification? entity)
    {
        if (entity == null) return null;
        
        var result = new Notification()
        {
            Id = entity.Id,
            FolderId = entity.FolderId,
            Message = entity.Message,
            PostedAt = entity.PostedAt,
        };
        
        return result;
    }

    public DAL.DTO.Notification? Map(Notification? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.Notification()
        {
            Id = entity.Id,
            FolderId = entity.FolderId,
            Message = entity.Message,
            PostedAt = entity.PostedAt,
        };
        
        return result;
    }
}