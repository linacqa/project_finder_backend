using Base.Contracts;
using DTO.v1.Identity;

namespace DTO.v1.Mappers;

public class CommentMapper : IMapper<DTO.v1.Comment, BLL.DTO.Comment>
{
    public Comment? Map(BLL.DTO.Comment? entity)
    {
        if (entity == null) return null;
        
        var result = new Comment()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            User = entity.User != null ? new UserInfo()
            {
                Id = entity.User.Id,
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName,
                Email = entity.User.Email,
            } : null,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new Project()
            {
                Id = entity.Project.Id,
                TitleInEstonian = entity.Project.TitleInEstonian,
                TitleInEnglish = entity.Project.TitleInEnglish,
            } : null,
            ReplyToCommentId = entity.ReplyToCommentId,
            Content = entity.Content,
            CreatedAt = entity.CreatedAt,
        };
        
        return result;
    }

    public BLL.DTO.Comment? Map(Comment? entity)
    {
        if (entity == null) return null;
        
        var result = new BLL.DTO.Comment()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            ProjectId = entity.ProjectId,
            ReplyToCommentId = entity.ReplyToCommentId,
            Content = entity.Content,
        };
        
        return result;
    }

    public BLL.DTO.Comment Map(CommentCreateUpdate entity)
    {
        var result = new BLL.DTO.Comment()
        {
            Id = Guid.NewGuid(),
            ProjectId = entity.ProjectId,
            ReplyToCommentId = entity.ReplyToCommentId,
            Content = entity.Content,
        };
        
        return result;
    }
}