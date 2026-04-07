using Base.BLL.Contracts;

namespace BLL.Contracts;

public interface IGroupService : IBaseService<BLL.DTO.Group>
{
    public Task<IEnumerable<BLL.DTO.Group>> AllAsyncMatchingTeamSize(int minStudents, int maxStudents, Guid userId = default);
}