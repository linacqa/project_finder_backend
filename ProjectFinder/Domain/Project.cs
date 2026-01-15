using Base.Domain;

namespace Domain;

public class Project : BaseEntity
{
    public Guid FolderId { get; set; }
    public Folder? Folder { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int MinStudents { get; set; }
    public int MaxStudents { get; set; }
    public DateTime Deadline { get; set; }
}