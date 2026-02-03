using Base.Domain;

namespace Domain;

public class ProjectType : BaseEntity
{
    public string Name { get; set; } = default!;
    
    public ICollection<Project>? Projects { get; set; }
}