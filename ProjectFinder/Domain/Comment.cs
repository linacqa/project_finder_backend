using Base.Domain;
using Domain.Identity;

namespace Domain;

public class Comment : BaseEntityUser<AppUser>
{
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    
    public Guid? ReplyToCommentId { get; set; }
    public Comment? ReplyToComment { get; set; }
    
    public string Content { get; set; } = default!;
}