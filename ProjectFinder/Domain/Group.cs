using Base.Domain;
using Domain.Identity;

namespace Domain;

public class Group : BaseEntityUser<AppUser>
{
    public string? Name { get; set; } = default!;
}