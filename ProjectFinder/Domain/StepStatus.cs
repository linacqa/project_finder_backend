using Base.Domain;

namespace Domain;

public class StepStatus : BaseEntity
{
    public string Name { get; set; } = default!;
    
    public ICollection<ProjectStep>? ProjectSteps { get; set; }
}