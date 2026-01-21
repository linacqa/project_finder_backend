using Base.Contracts;

namespace DTO.v1;

public class Tag : IDomainId
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;
}