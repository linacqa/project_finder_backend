using Base.Contracts;
using DAL.DTO;

namespace DAL.EF.Mappers;

public class NotificationUOWMapper : IMapper<DAL.DTO.Notification, Domain.Notification>
{
    public Notification? Map(Domain.Notification? entity)
    {
        if (entity == null) return null;

        return new Notification()
        {
            Id = entity.Id,
            FolderId = entity.FolderId,
            Folder = entity.Folder != null ? new Folder()
            {
                Id = entity.Folder.Id,
                Name = entity.Folder.Name,
            } : null,
            Message = entity.Message,
            PostedAt = entity.PostedAt,
        };
    }

    public Domain.Notification? Map(Notification? entity)
    {
        if (entity == null) return null;

        return new Domain.Notification()
        {
            Id = entity.Id,
            FolderId = entity.FolderId,
            Folder = entity.Folder != null ? new Domain.Folder()
            {
                Id = entity.Folder.Id,
                Name = entity.Folder.Name,
            } : null,
            Message = entity.Message,
            PostedAt = entity.PostedAt,
        };
    }
}