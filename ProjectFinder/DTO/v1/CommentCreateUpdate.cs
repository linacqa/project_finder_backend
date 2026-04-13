using System.ComponentModel.DataAnnotations;

namespace DTO.v1;

public class CommentCreateUpdate
{
    public Guid ProjectId { get; set; }
    
    public Guid? ReplyToCommentId { get; set; }
    
    [Required]
    [MaxLength(2500)]
    public string Content { get; set; } = default!;
}