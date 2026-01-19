using Base.Contracts;
using BLL.DTO.Identity;

namespace BLL.DTO;

public class Comment : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    
    public Guid? ReplyToCommentId { get; set; }
    public Comment? ReplyToComment { get; set; }
    
    public string Content { get; set; } = default!;
    
    public ICollection<Comment>? Replies { get; set; }
}