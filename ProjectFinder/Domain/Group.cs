using Base.Domain;
using Domain.Identity;

namespace Domain;

public class Group : BaseEntityUser<AppUser>
{
    public string? Name { get; set; } = default!;
    
    public ICollection<Application>? Applications { get; set; }
    public ICollection<Invitation>? Invitations { get; set; }
    public ICollection<AppUser>? Members { get; set; }
}