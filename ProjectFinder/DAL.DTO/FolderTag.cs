using Base.Contracts;

namespace DAL.DTO;

public class FolderTag : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid FolderId { get; set; }
    public Folder? Folder { get; set; }

    public Guid TagId { get; set; }
    public Tag? Tag { get; set; }
}