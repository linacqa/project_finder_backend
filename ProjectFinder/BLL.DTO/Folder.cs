using Base.Contracts;

namespace BLL.DTO;

public class Folder : IDomainId
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;
    public bool IsVisible { get; set; }
    
    public ICollection<ProjectFolder>? ProjectFolders { get; set; }
    public ICollection<FolderTag>? FolderTags { get; set; }
    public ICollection<Notification>? Notifications { get; set; }
}