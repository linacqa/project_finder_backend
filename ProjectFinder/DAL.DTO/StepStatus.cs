using Base.Contracts;

namespace DAL.DTO;

public class StepStatus : IDomainId
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;
    
    public ICollection<ProjectStep>? ProjectSteps { get; set; }
}