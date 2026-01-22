namespace DTO.v1;

public class NotificationCreate
{
    public Guid? FolderId { get; set; }
    
    public string Message { get; set; } = default!;
    public DateTime PostAt { get; set; }
}