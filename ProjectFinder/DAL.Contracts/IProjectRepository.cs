using Base.DAL.Contracts;
using DTO.v1;
using Project = DAL.DTO.Project;

namespace DAL.Contracts;

public interface IProjectRepository : IBaseRepository<Project>
{
    Task<IEnumerable<Project>> AllCurrentUserAsync(Guid userId = default);
    Task<(IEnumerable<Project>, int)> SearchAsync(ProjectsSearchRequest request, Guid userId = default);
}