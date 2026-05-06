using System.ComponentModel.DataAnnotations;

namespace DTO.v1.Identity;

public class UpdateAccountInfo
{
    [MaxLength(128)]
    public string FirstName { get; set; } = default!;
    
    [MaxLength(128)]
    public string LastName { get; set; } = default!;

    [MaxLength(32)]
    public string? PhoneNumber { get; set; }

    [MaxLength(128)]
    public string Email { get; set; } = default!;

    [MaxLength(16)]
    public string? UniId { get; set; }

    [MaxLength(16)]
    public string? MatriculationNumber { get; set; }

    [MaxLength(128)]
    public string? Program { get; set; }
}