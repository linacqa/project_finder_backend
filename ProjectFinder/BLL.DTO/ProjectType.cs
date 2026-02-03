using Base.Contracts;

namespace BLL.DTO;

public class ProjectType : IDomainId
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;
    
    public ICollection<Project>? Projects { get; set; }
}