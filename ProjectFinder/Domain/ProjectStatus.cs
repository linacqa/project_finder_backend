using Base.Domain;

namespace Domain;

public class ProjectStatus : BaseEntity
{
    public string Name { get; set; } = default!;
    
    public ICollection<Project>? Projects { get; set; }
}