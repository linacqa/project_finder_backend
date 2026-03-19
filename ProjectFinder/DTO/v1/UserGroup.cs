using Base.Contracts;
using DTO.v1.Identity;

namespace DTO.v1;

public class UserGroup : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public UserInfo? User { get; set; }
    
    public Guid GroupId { get; set; }
    
    public string? Role { get; set; } = default!;
}