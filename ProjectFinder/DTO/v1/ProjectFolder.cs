using Base.Contracts;

namespace DTO.v1;

public class ProjectFolder : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid ProjectId { get; set; }
    
    public Guid FolderId { get; set; }
}