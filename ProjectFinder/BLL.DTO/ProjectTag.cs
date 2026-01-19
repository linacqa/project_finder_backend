using Base.Contracts;

namespace BLL.DTO;

public class ProjectTag : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    
    public Guid TagId { get; set; }
    public Tag? Tag { get; set; }
}