using Base.Domain;

namespace Domain;

public class ProjectFolder : BaseEntity
{
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    
    public Guid FolderId { get; set; }
    public Folder? Folder { get; set; }
}