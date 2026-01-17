using Base.Domain;
using Domain.Identity;

namespace Domain;

public class Tag : BaseEntity
{
    public string Name { get; set; } = default!;
    
    public ICollection<Folder>? Folders { get; set; }
    public  ICollection<Project>? Projects { get; set; }
    public ICollection<AppUser>? Users { get; set; }
}