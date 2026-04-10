using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class CommentRepository : BaseRepository<Comment, Domain.Comment>, ICommentRepository
{
    public CommentRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new CommentUOWMapper())
    {
    }
    
    public async Task<IEnumerable<DAL.DTO.Comment>> AllAsyncByProjectId(Guid projectId, Guid userId)
    {
        var res = await RepositoryDbSet.AsQueryable()
            .Where(e => e.ProjectId.Equals(projectId))
            .Include(e => e.User)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();

        return res.Select(e => Mapper.Map(e))!;
    }
}