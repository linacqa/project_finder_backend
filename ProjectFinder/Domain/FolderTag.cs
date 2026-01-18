using Base.Domain;

namespace Domain;

public class FolderTag : BaseEntity
{
    public Guid FolderId { get; set; }
    public Folder? Folder { get; set; }

    public Guid TagId { get; set; }
    public Tag? Tag { get; set; }
}