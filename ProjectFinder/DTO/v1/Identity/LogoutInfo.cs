using System.ComponentModel.DataAnnotations;

namespace DTO.v1.Identity;

public class LogoutInfo
{
    [Required]
    public string RefreshToken { get; set; } = default!;
}