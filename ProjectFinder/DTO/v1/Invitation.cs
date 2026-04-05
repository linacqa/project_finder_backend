using Base.Contracts;
using DTO.v1.Identity;

namespace DTO.v1;

public class Invitation : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid GroupId { get; set; }
    
    public Group? Group { get; set; }
    
    // public Guid ProjectId { get; set; }
    
    public Guid ToUserId { get; set; }
    
    public StudentInfo? ToUser { get; set; }
    
    public Guid FromUserId { get; set; }
    
    public StudentInfo? FromUser { get; set; }
    
    public string Role { get; set; } = default!;
    
    public DateTime? AcceptedAt { get; set; }
    public DateTime? DeclinedAt { get; set; }
}