using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class UserProjectRepository : BaseRepository<UserProject, Domain.UserProject>, IUserProjectRepository
{
    public UserProjectRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new UserProjectUOWMapper())
    {
    }
    
    public Task<bool> UserInProject(Guid projectId, Guid userId)
    {
        return RepositoryDbContext.Set<Domain.UserProject>().AnyAsync(up => up.ProjectId == projectId && up.UserId == userId);
    }
}