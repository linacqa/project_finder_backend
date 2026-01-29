using System.ComponentModel.DataAnnotations;

namespace DTO.v1.Identity;

public class RegisterInfo
{
    [Required]
    [MaxLength(128)]
    public string Email { get; set; } = default!;

    [Required]
    [MaxLength(128)]
    public string Password { get; set; } = default!;
    
    [MinLength(1)]
    [MaxLength(128)]
    public string FirstName { get; set; } = default!;
        
    [MinLength(1)]
    [MaxLength(128)]
    public string LastName { get; set; } = default!;
    
    [MinLength(1)]
    [MaxLength(64)]
    public string Role { get; set; } = "user";
}