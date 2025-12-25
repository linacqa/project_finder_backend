using System.ComponentModel.DataAnnotations;
using Base.Contracts;

namespace Base.Domain;

public abstract class BaseEntity : BaseEntity<Guid>, IDomainId
{
}

public abstract class BaseEntity<TKey> : IDomainId<TKey>, IDomainMeta
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;
    
    [MaxLength(64)]
    public string CreatedBy { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    
    [MaxLength(64)]
    public string? ChangedBy { get; set; }
    public DateTime? ChangedAt { get; set; }
    
    public string? SysNotes { get; set; }
}