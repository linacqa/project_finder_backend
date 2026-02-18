using Base.Contracts;
using Domain.Identity;

namespace DAL.DTO.Identity;

public class AppUser : IDomainId
{
    public Guid Id { get; set; }
    
    public string FirstName { get; set; } = default!;
    
    public string LastName { get; set; } = default!;
    
    public string? Email { get; set; }
    
    public string? AzureObjectId { get; set; }

    public AuthType AuthType { get; set; }
    
    public ICollection<UserProject>? UserProjects { get; set; }
    public ICollection<UserGroup>? UserGroups { get; set; }
    public ICollection<Application>? Applications { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<UserTag>? UserTags { get; set; }
    public ICollection<Project>? Projects { get; set; }
    public ICollection<Group>? Groups { get; set; }
    
    public ICollection<Invitation>? OutgoingInvitations { get; set; }
    public ICollection<Invitation>? IncomingInvitations { get; set; }
}