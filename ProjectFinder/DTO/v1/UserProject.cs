using Base.Contracts;
using DTO.v1.Identity;

namespace DTO.v1;

public class UserProject : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public UserInfo? User { get; set; }
    
    public Guid ProjectId { get; set; }
    
    public Guid UserProjectRoleId { get; set; }
    public UserProjectRole? UserProjectRole { get; set; }
}