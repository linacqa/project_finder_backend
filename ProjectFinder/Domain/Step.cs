using Base.Domain;

namespace Domain;

public class Step : BaseEntity
{
    public string? Name { get; set; } = default!;
}