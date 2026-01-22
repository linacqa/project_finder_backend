using Base.Contracts;

namespace DTO.v1.Mappers;

public class NotificationMapper : IMapper<DTO.v1.Notification, BLL.DTO.Notification>
{
    public Notification? Map(BLL.DTO.Notification? entity)
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

    public BLL.DTO.Notification? Map(Notification? entity)
    {
        if (entity == null) return null;
        
        var result = new BLL.DTO.Notification()
        {
            Id = entity.Id,
            FolderId = entity.FolderId,
            Message = entity.Message,
            PostedAt = entity.PostedAt,
        };
        
        return result;
    }

    public BLL.DTO.Notification Map(NotificationCreate entity)
    {
        var result = new BLL.DTO.Notification()
        {
            Id = Guid.NewGuid(),
            FolderId = entity.FolderId,
            Message = entity.Message,
            PostedAt = entity.PostAt,
        };
        
        return result;
    }
}