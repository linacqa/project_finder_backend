using Base.Domain;

namespace Domain;

public class Folder : BaseEntity
{
    public string Name { get; set; } = default!;
    public bool IsVisible { get; set; }
    
    public ICollection<Project>? Projects { get; set; }
    public ICollection<Tag>? Tags { get; set; }
    public ICollection<Notification>? Notifications { get; set; }
}