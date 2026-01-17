using Base.Domain;
using Domain.Identity;

namespace Domain;

public class Application : BaseEntity
{
    public Guid? UserId { get; set; }
    public AppUser? User { get; set; }
    
    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }
    
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    
    public DateTime AcceptedAt { get; set; }
    public DateTime DeclinedAt { get; set; }
}