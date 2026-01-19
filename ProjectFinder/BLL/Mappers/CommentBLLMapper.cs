using Base.Contracts;
using BLL.DTO;

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
            ProjectId = entity.ProjectId,
            Content = entity.Content,
            ReplyToCommentId = entity.ReplyToCommentId,
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