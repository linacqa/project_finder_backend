namespace DTO.v1;

public class InvitationCreate
{
    public Guid GroupId { get; set; }
    
    public Guid ToUserId { get; set; }
    
    public string Role { get; set; } = default!;
}