namespace DTO.v1;

public class CommentCreateUpdate
{
    public Guid ProjectId { get; set; }
    
    public Guid? ReplyToCommentId { get; set; }
    
    public string Content { get; set; } = default!;
}