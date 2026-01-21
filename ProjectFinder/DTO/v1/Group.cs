using Base.Contracts;

namespace DTO.v1;

public class Group : IDomainId
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; } = default!;
    public bool IsAzureAdGroup { get; set; }
    
    public Guid CreatorId { get; set; }
}