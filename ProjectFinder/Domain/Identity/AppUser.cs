using System.ComponentModel.DataAnnotations;
using Base.Domain.Identity;

namespace Domain.Identity;

public class AppUser : BaseUser<AppUserRole>
{
    [MinLength(1)]
    [MaxLength(128)]
    public string FirstName { get; set; } = default!;
 
    [MinLength(1)]
    [MaxLength(128)]
    public string LastName { get; set; } = default!;
    
    [MaxLength(15)]
    public string? UniId { get; set; }
    
    [MaxLength(15)]
    public string? MatriculationNumber { get; set; }
    
    [MaxLength(128)]
    public string? Program { get; set; }
    
    [MaxLength(450)]
    public string? AzureObjectId { get; set; }

    public AuthType AuthType { get; set; }

    
    public ICollection<AppRefreshToken>? RefreshTokens { get; set; }
    
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