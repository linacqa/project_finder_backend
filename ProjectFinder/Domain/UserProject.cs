using Base.Domain;
using Domain.Identity;

namespace Domain;

public class UserProject : BaseEntityUser<AppUser>
{
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
}