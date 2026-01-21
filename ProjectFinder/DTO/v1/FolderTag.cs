using Base.Contracts;

namespace DTO.v1;

public class FolderTag : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid FolderId { get; set; }

    public Guid TagId { get; set; }
}