using Base.DAL.Contracts;
using DAL.DTO;

namespace DAL.Contracts;

public interface IInvitationRepository : IBaseRepository<Invitation>
{
    public Task<IEnumerable<Invitation>> AllAsyncToUser(Guid userId = default);
}