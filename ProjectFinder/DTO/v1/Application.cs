using Base.Contracts;

namespace DTO.v1;

public class Application : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid? UserId { get; set; }
    
    public Guid? GroupId { get; set; }
    
    public Guid ProjectId { get; set; }
    
    public DateTime? AcceptedAt { get; set; }
    public DateTime? DeclinedAt { get; set; }
}