using Base.Domain;
using Domain.Identity;

namespace Domain;

public class UserTag : BaseEntityUser<AppUser>
{
    public Guid TagId { get; set; }
    public Tag? Tag { get; set; }
}