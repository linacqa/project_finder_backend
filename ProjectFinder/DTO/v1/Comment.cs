using Base.Contracts;

namespace DTO.v1;

public class Comment : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid ProjectId { get; set; }
    
    public Guid? ReplyToCommentId { get; set; }
    // public Comment? ReplyToComment { get; set; }
    
    public string Content { get; set; } = default!;
}