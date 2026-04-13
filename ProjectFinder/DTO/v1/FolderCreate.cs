using System.ComponentModel.DataAnnotations;

namespace DTO.v1;

public class FolderCreate
{
    [Required]
    [MaxLength(64)]
    public string Name { get; set; } = default!;
    public bool IsVisible { get; set; }
}