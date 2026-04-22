using Base.BLL.Contracts;

namespace BLL.Contracts;

public interface IInvitationService : IBaseService<BLL.DTO.Invitation>
{
    public Task<IEnumerable<BLL.DTO.Invitation>> AllAsyncToUser(Guid userId = default);
    Task<IEnumerable<BLL.DTO.Invitation>> AllAsyncByGroupId(Guid groupId, Guid userId = default);
}