using Base.Contracts;
using BLL.DTO.Identity;
using Domain;

namespace BLL.DTO;

public class Project : IDomainId
{
    public Guid Id { get; set; }
    public string TitleInEstonian { get; set; } = default!;
    public string? TitleInEnglish { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? Client { get; set; } = default!;
    public string? ExternalSupervisor { get; set; } = default!;
    public int MinStudents { get; set; }
    public int MaxStudents { get; set; }
    
    public Guid ProjectTypeId { get; set; }
    public ProjectType? ProjectType { get; set; }
    
    public Guid ProjectStatusId { get; set; }
    public ProjectStatus? ProjectStatus { get; set; }
    
    public DateTime? Deadline { get; set; }
    public List<string> AttachmentsPaths { get; set; } = [];
    
    public List<Guid> FolderIds { get; set; } = new ();
    public List<Guid> TagIds { get; set; } = new ();
    public List<Guid> StepIds { get; set; } = new ();
    
    public Guid AuthorId { get; set; }
    public Guid? ExternalSupervisorId { get; set; }
    public Guid? PrimarySupervisorId { get; set; }
    public string? PrimarySupervisor { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public ICollection<ProjectFolder>? ProjectFolders { get; set; }
    public ICollection<ProjectTag>? ProjectTags { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<Application>? Applications { get; set; }
    public ICollection<Invitation>? Invitations { get; set; }
    public ICollection<ProjectStep>? ProjectSteps { get; set; }
    public ICollection<GroupProject>? GroupProjects { get; set; }
    public ICollection<UserProject>? UserProjects { get; set; }
}