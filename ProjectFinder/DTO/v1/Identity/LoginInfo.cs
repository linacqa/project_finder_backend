using System.ComponentModel.DataAnnotations;

namespace DTO.v1.Identity;

public class LoginInfo
{
    [Required]
    [MaxLength(128)]
    public string Email { get; set; } = default!;
    
    [Required]
    [MaxLength(128)]
    public string Password { get; set; } = default!;
}