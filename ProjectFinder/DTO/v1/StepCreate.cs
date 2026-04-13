using System.ComponentModel.DataAnnotations;

namespace DTO.v1;

public class StepCreate
{
    [Required]
    [MaxLength(64)]
    public string Name { get; set; } = default!;
}