using Base.Contracts;
using BLL.DTO.Identity;

namespace BLL.DTO;

public class Invitation : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid GroupId { get; set; }
    public Group? Group { get; set; }
    
    // public Guid ProjectId { get; set; }
    // public Project? Project { get; set; }
    
    public Guid ToUserId { get; set; }
    public AppUser? ToUser { get; set; }
    
    public Guid FromUserId { get; set; }
    public AppUser? FromUser { get; set; }
    
    public string Role { get; set; } = default!;
    
    public DateTime? AcceptedAt { get; set; }
    public DateTime? DeclinedAt { get; set; }
}