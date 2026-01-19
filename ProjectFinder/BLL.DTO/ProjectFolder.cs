using Base.Contracts;

namespace BLL.DTO;

public class ProjectFolder : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    
    public Guid FolderId { get; set; }
    public Folder? Folder { get; set; }
}