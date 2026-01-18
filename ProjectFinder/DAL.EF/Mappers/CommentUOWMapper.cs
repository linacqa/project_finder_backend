using Base.Contracts;
using DAL.DTO;
using DAL.EF.Mappers.Identity;

namespace DAL.EF.Mappers;

public class CommentUOWMapper : IMapper<DAL.DTO.Comment, Domain.Comment>
{
    public Comment? Map(Domain.Comment? entity)
    {
        if (entity == null) return null;
        
        return new Comment()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            User = entity.User != null ? new AppUserUOWMapper().Map(entity.User) : null,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new ProjectUOWMapper().Map(entity.Project) : null,
            ReplyToCommentId = entity.ReplyToCommentId,
            ReplyToComment = entity.ReplyToComment != null ? Map(entity.ReplyToComment) : null,
            Content = entity.Content,
        };
    }

    public Domain.Comment? Map(Comment? entity)
    {
        if (entity == null) return null;
        
        return new Domain.Comment()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            User = entity.User != null ? new AppUserUOWMapper().Map(entity.User) : null,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new ProjectUOWMapper().Map(entity.Project) : null,
            ReplyToCommentId = entity.ReplyToCommentId,
            ReplyToComment = entity.ReplyToComment != null ? Map(entity.ReplyToComment) : null,
            Content = entity.Content,
        };
    }
}