using Base.Domain;
using Domain.Identity;

namespace Domain;

public class Application : BaseEntityUser<AppUser>
{
    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }
    
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
}