using Base.Contracts;

namespace DTO.v1;

public class Notification : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid? FolderId { get; set; }
    
    public string Message { get; set; } = default!;
    public DateTime PostedAt { get; set; }
}