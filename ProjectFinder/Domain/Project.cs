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
    
    public ICollection<Tag>? Tags { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<Application>? Applications { get; set; }
    public ICollection<Invitation>? Invitations { get; set; }
}