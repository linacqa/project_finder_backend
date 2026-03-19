using Base.Contracts;
using DTO.v1.Identity;

namespace DTO.v1;

public class Group : IDomainId
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; } = default!;
    public bool IsAzureAdGroup { get; set; }
    
    public Guid CreatorId { get; set; }
    public UserInfo? Creator { get; set; }
    
    public ICollection<UserGroup> Users { get; set; }
}