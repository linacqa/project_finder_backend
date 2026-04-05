using Base.Domain;
using Domain.Identity;

namespace Domain;

public class Invitation : BaseEntityUser<AppUser>
{
    public Guid GroupId { get; set; }
    public Group? Group { get; set; }
    
    // public Guid ProjectId { get; set; }
    // public Project? Project { get; set; }
    
    public Guid ToUserId { get; set; }
    public AppUser? ToUser { get; set; }
    
    public string Role { get; set; } = default!;
    
    public DateTime? AcceptedAt { get; set; }
    public DateTime? DeclinedAt { get; set; }
}