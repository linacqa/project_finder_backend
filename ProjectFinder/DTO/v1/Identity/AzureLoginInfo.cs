using System.ComponentModel.DataAnnotations;

namespace DTO.v1.Identity;

public class AzureLoginInfo
{
    [Required]
    public string AccessToken { get; set; } = default!;
}