using Base.Contracts;
using Domain;

namespace DTO.v1;

public class Project : IDomainId
{
    public Guid Id { get; set; }
    public string TitleInEstonian { get; set; } = default!;
    public string? TitleInEnglish { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? Client { get; set; } = default!;
    public string? Supervisor { get; set; } = default!;
    public string? ExternalSupervisor { get; set; } = default!;
    public int MinStudents { get; set; }
    public int MaxStudents { get; set; }
    public Guid ProjectTypeId { get; set; }
    public ProjectType? ProjectType { get; set; }
    public Guid ProjectStatusId { get; set; }
    public ProjectStatus? ProjectStatus { get; set; }
    public DateTime? Deadline { get; set; }
    public List<string> AttachmentsPaths { get; set; } = [];
    
    public ICollection<Tag>? Tags { get; set; }
    public ICollection<UserProject>? Users { get; set; }
}