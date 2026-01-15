using Base.Domain;

namespace Domain;

public class Folder : BaseEntity
{
    public string Name { get; set; } = default!;
    public bool IsVisible { get; set; }
    
    public ICollection<Project>? Projects { get; set; }
}