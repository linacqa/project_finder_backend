using Base.Contracts;

namespace DAL.DTO;

public class UserProjectRole : IDomainId
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    
    public ICollection<UserProject>? UserProjects { get; set; }
}