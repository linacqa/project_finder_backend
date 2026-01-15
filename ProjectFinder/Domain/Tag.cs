using Base.Domain;

namespace Domain;

public class Tag : BaseEntity
{
    public string Name { get; set; } = default!;
}