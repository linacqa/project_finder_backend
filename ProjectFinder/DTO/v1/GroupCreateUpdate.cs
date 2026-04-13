using System.ComponentModel.DataAnnotations;

namespace DTO.v1;

public class GroupCreateUpdate
{
    [Required]
    [MaxLength(64)]
    public string Name { get; set; } = default!;
    
    [Required]
    [MaxLength(32)]
    public string CreatorRoleInGroup { get; set; } = default!;
}