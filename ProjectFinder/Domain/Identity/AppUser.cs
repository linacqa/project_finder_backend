using System.ComponentModel.DataAnnotations;
using Base.Domain.Identity;

namespace Domain.Identity;

public class AppUser : BaseUser<AppUserRole>
{
    /*public ICollection<Category>? Categories { get; set; }
    public ICollection<Task>? Tasks { get; set; }
    public ICollection<ContactType>? ContactTypes { get; set; }
    public ICollection<Note>? Notes { get; set; }
    public ICollection<File>? Files { get; set; }*/
    [MinLength(1)]
    [MaxLength(128)]
    public string FirstName { get; set; } = default!;
 
    [MinLength(1)]
    [MaxLength(128)]
    public string LastName { get; set; } = default!;
    
    [MaxLength(450)]
    public string? AzureObjectId { get; set; }

    public AuthType AuthType { get; set; }

    
    public ICollection<AppRefreshToken>? RefreshTokens { get; set; }
}