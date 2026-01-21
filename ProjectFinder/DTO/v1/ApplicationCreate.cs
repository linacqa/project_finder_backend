namespace DTO.v1;

public class ApplicationCreate
{
    public Guid? UserId { get; set; }
    
    public Guid? GroupId { get; set; }
    
    public Guid ProjectId { get; set; }
}