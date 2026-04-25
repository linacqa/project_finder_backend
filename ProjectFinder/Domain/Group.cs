using Base.Domain;
using Domain.Identity;

namespace Domain;

public class Group : BaseEntityUser<AppUser>
{
    public string? Name { get; set; } = default!;
    public bool IsAzureAdGroup { get; set; }
    
    public ICollection<Application>? Applications { get; set; }
    public ICollection<Invitation>? Invitations { get; set; }
    public ICollection<UserGroup>? UserGroups { get; set; }
}