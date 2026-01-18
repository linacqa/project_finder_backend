using Base.Contracts;

namespace DAL.DTO;

public class Tag : IDomainId
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;
    
    public ICollection<FolderTag>? FolderTags { get; set; }
    public  ICollection<ProjectTag>? ProjectTags { get; set; }
    public ICollection<UserTag>? UserTags { get; set; }
}