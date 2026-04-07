using Base.DAL.Contracts;
using DAL.DTO;

namespace DAL.Contracts;

public interface IGroupRepository : IBaseRepository<Group>
{
    public Task<IEnumerable<Group>> AllAsyncMatchingTeamSize(int minStudents, int maxStudents,
        Guid userId = default);
}