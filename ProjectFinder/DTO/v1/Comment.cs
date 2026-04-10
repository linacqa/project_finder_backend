using Base.Contracts;
using DTO.v1.Identity;

namespace DTO.v1;

public class Comment : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public UserInfo? User { get; set; }
    
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    
    public Guid? ReplyToCommentId { get; set; }
    // public Comment? ReplyToComment { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string Content { get; set; } = default!;
}