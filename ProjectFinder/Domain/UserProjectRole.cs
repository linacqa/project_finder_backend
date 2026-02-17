using Base.Domain;

namespace Domain;

public class UserProjectRole : BaseEntity
{
    public string Name { get; set; } = default!;
    
    public ICollection<UserProject>? UserProjects { get; set; }
}