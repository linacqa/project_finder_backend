using System.ComponentModel.DataAnnotations;

namespace DTO.v1;

public class NotificationCreate
{
    public Guid? FolderId { get; set; }
    
    [Required]
    [MaxLength(5000)]
    public string Message { get; set; } = default!;
    public DateTime PostAt { get; set; }
}