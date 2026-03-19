using Base.Domain;
using Domain.Identity;

namespace Domain;

public class UserGroup : BaseEntityUser<AppUser>
{
    public Guid GroupId { get; set; }
    public Group? Group { get; set; }
    
    public string? Role { get; set; } = default!;
}