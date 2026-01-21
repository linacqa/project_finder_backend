using Base.Contracts;

namespace DTO.v1;

public class Folder : IDomainId
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;
    public bool IsVisible { get; set; }
}