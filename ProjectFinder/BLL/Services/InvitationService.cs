using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class InvitationService : BaseService<BLL.DTO.Invitation, DAL.DTO.Invitation, DAL.Contracts.IInvitationRepository>, IInvitationService
{
    public InvitationService(
        IAppUOW serviceUOW, 
        IMapper<Invitation, DAL.DTO.Invitation, Guid> mapper) : base(serviceUOW, serviceUOW.InvitationRepository, mapper)
    {
    }
}