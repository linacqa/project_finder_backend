using System.ComponentModel.DataAnnotations;

namespace DTO.v1;

public class InvitationCreate
{
    public Guid GroupId { get; set; }
    
    public Guid ToUserId { get; set; }
    
    [Required]
    [MaxLength(32)]
    public string Role { get; set; } = default!;
}