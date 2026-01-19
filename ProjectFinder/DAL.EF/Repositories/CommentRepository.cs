using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class CommentRepository : BaseRepository<Comment, Domain.Comment>, ICommentRepository
{
    public CommentRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new CommentUOWMapper())
    {
    }
}