using Base.Contracts;

namespace DTO.v1;

public class Invitation : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid GroupId { get; set; }
    
    public Guid ProjectId { get; set; }
    
    public Guid ToUserId { get; set; }
    
    public Guid FromUserId { get; set; }
    
    public string Role { get; set; } = default!;
    
    public DateTime? AcceptedAt { get; set; }
    public DateTime? DeclinedAt { get; set; }
}