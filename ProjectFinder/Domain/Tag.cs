using Base.Domain;
using Domain.Identity;

namespace Domain;

public class Tag : BaseEntity
{
    public string Name { get; set; } = default!;
    
    public ICollection<FolderTag>? FolderTags { get; set; }
    public  ICollection<ProjectTag>? ProjectTags { get; set; }
    public ICollection<UserTag>? UserTags { get; set; }
}