using Base.Domain;
using Domain.Identity;

namespace Domain;

public class Project : BaseEntityUser<AppUser>
{
    public Guid FolderId { get; set; }
    public Folder? Folder { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? Client { get; set; } = default!;
    public int MinStudents { get; set; }
    public int MaxStudents { get; set; }
    public ProjectType ProjectType { get; set; }
    public ProjectStatus ProjectStatus { get; set; }
    public DateTime Deadline { get; set; }
    
    public ICollection<Tag>? Tags { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<Application>? Applications { get; set; }
    public ICollection<Invitation>? Invitations { get; set; }
    public ICollection<ProjectStep>? ProjectSteps { get; set; }
    public ICollection<Group>? Groups { get; set; }
    public ICollection<AppUser>? Users { get; set; }
}