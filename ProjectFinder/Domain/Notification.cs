using Base.Domain;

namespace Domain;

public class Notification : BaseEntity
{
    public Guid? FolderId { get; set; }
    public Folder? Folder { get; set; }
    
    public string Message { get; set; } = default!;
    public DateTime PostedAt { get; set; }
}