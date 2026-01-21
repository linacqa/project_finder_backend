using Base.Contracts;

namespace DTO.v1;

public class ProjectTag : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid ProjectId { get; set; }
    
    public Guid TagId { get; set; }
}