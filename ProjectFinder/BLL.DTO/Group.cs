using Base.Contracts;
using BLL.DTO.Identity;

namespace BLL.DTO;

public class Group : IDomainId
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; } = default!;
    public bool IsAzureAdGroup { get; set; }
    
    public Guid CreatorId { get; set; }
    public AppUser? Creator { get; set; }
    
    public ICollection<Application>? Applications { get; set; }
    public ICollection<Invitation>? Invitations { get; set; }
    public ICollection<UserGroup>? UserGroups { get; set; }
    public ICollection<GroupProject>? GroupProjects { get; set; }
}