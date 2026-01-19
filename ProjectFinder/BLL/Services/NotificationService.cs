using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class NotificationService : BaseService<BLL.DTO.Notification, DAL.DTO.Notification, DAL.Contracts.INotificationRepository>, INotificationService
{
    public NotificationService(
        IAppUOW serviceUOW, 
        IMapper<Notification, DAL.DTO.Notification, Guid> mapper) : base(serviceUOW, serviceUOW.NotificationRepository, mapper)
    {
    }
}