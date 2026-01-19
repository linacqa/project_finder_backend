using Base.Contracts;

namespace BLL.DTO;

public class Notification : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid? FolderId { get; set; }
    public Folder? Folder { get; set; }
    
    public string Message { get; set; } = default!;
    public DateTime PostedAt { get; set; }
}