using Base.Domain;

namespace Domain;

public class Folder : BaseEntity
{
    public string Name { get; set; } = default!;
    public bool IsVisible { get; set; }
    
    public ICollection<ProjectFolder>? ProjectFolders { get; set; }
    public ICollection<FolderTag>? FolderTags { get; set; }
    public ICollection<Notification>? Notifications { get; set; }
}