using Base.Contracts;
using DTO.v1.Identity;

namespace DTO.v1;

public class Application : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid? UserId { get; set; }
    public UserInfo? User { get; set; }
    
    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }
    
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    
    public DateTime? AcceptedAt { get; set; }
    public DateTime? DeclinedAt { get; set; }
}