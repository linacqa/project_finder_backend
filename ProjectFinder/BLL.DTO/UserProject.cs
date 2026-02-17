using Base.Contracts;
using BLL.DTO.Identity;

namespace BLL.DTO;

public class UserProject : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    
    public Guid UserProjectRoleId { get; set; }
    public UserProjectRole? UserProjectRole { get; set; }
}