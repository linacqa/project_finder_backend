using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class NotificationRepository : BaseRepository<Notification, Domain.Notification>, INotificationRepository
{
    public NotificationRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new NotificationUOWMapper())
    {
    }
}