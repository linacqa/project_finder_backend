using System.ComponentModel.DataAnnotations;

namespace DTO.v1.Identity;

public class AdminEmailRequest
{
    [MaxLength(200)] public string Subject { get; set; } = "Uus projekti idee";

    [Required]
    [MinLength(1)]
    [MaxLength(10000)]
    public string Body { get; set; } = default!;
}

