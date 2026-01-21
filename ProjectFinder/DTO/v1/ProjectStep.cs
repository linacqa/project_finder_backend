using Base.Contracts;
using Domain;

namespace DTO.v1;

public class ProjectStep : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    
    public Guid StepId { get; set; }
    
    public StepStatus StepStatus { get; set; }
    public int Order { get; set; }
}