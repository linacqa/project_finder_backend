using System.ComponentModel.DataAnnotations;
using Domain;

namespace DTO.v1;

public class ProjectCreate
{
    [Required]
    [MaxLength(128)]
    public string TitleInEstonian { get; set; } = default!;
    [MaxLength(128)]
    public string? TitleInEnglish { get; set; } = default!;
    [Required]
    [MaxLength(5000)]
    public string Description { get; set; } = default!;
    [MaxLength(128)]
    public string? Client { get; set; } = default!;
    [MaxLength(128)]
    public string? ExternalSupervisor { get; set; } = default!;
    [Required]
    [Range(1, 10)]
    public int MinStudents { get; set; }
    [Required]
    [Range(1, 10)]
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
    [MaxLength(128)]
    public string? PrimarySupervisor { get; set; }
    // public List<string> AttachmentsPaths { get; set; } = [];
    
    // public Guid CreatorId { get; set; }
}