using Base.Contracts;
using BLL.DTO;
using BLL.DTO.Identity;

namespace BLL.Mappers;

public class CommentBLLMapper : IMapper<BLL.DTO.Comment, DAL.DTO.Comment>
{
    public Comment? Map(DAL.DTO.Comment? entity)
    {
        if (entity == null) return null;
        
        var result = new Comment()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            User = entity.User != null ? new AppUser()
            {
                Id = entity.User.Id,
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName,
                Email = entity.User.Email,
            } : null,
            ProjectId = entity.ProjectId,
            Content = entity.Content,
            ReplyToCommentId = entity.ReplyToCommentId,
            CreatedAt = entity.CreatedAt,
        };
        
        return result;
    }

    public DAL.DTO.Comment? Map(Comment? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.Comment()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            ProjectId = entity.ProjectId,
            Content = entity.Content,
            ReplyToCommentId = entity.ReplyToCommentId,
        };
        
        return result;
    }
}