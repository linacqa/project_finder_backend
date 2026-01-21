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
    public ProjectType ProjectType { get; set; }
    public ProjectStatus ProjectStatus { get; set; }
    public DateTime? Deadline { get; set; }
    // public List<string> AttachmentsPaths { get; set; } = [];
    
    // public Guid CreatorId { get; set; }
}