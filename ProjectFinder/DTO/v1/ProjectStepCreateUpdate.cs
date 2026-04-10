namespace DTO.v1;

public class ProjectStepCreateUpdate
{
    public Guid ProjectId { get; set; }
    
    public Guid StepId { get; set; }
    
    public Guid StepStatusId { get; set; }
    
    public int Order { get; set; }
}