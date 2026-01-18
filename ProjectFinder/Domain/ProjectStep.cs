using Base.Domain;

namespace Domain;

public class ProjectStep : BaseEntity
{
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    
    public Guid StepId { get; set; }
    public Step? Step { get; set; }
    
    public StepStatus StepStatus { get; set; }
}