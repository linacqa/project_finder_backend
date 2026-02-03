using Base.Contracts;
using Domain;

namespace BLL.DTO;

public class ProjectStep : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    
    public Guid StepId { get; set; }
    public Step? Step { get; set; }
    
    public Guid StepStatusId { get; set; }
    public StepStatus? StepStatus { get; set; }
    public int Order { get; set; }
}