using Domain;

namespace DTO.v1;

public class ProjectCreate
{
    public string TitleInEstonian { get; set; } = default!;
    public string? TitleInEnglish { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? Client { get; set; } = default!;
    public string? ExternalSupervisor { get; set; } = default!;
    public int MinStudents { get; set; }
    public int MaxStudents { get; set; }
    public Guid ProjectTypeId { get; set; }
    public Guid ProjectStatusId { get; set; }
    public DateTime? Deadline { get; set; }
    public List<Guid> FolderIds { get; set; } = new ();
    public List<Guid> TagIds { get; set; } = new ();
    public List<Guid> StepIds { get; set; } = new ();
    
    public Guid AuthorId { get; set; }
    public Guid? ExternalSupervisorId { get; set; }
    public Guid? PrimarySupervisorId { get; set; }
    public string? PrimarySupervisor { get; set; }
    // public List<string> AttachmentsPaths { get; set; } = [];
    
    // public Guid CreatorId { get; set; }
}