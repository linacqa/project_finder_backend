using System.ComponentModel.DataAnnotations;

namespace Base.Domain.Identity;

public class BaseRefreshToken : BaseRefreshToken<Guid>
{
    
}


public class BaseRefreshToken<TKey> : BaseEntity<TKey> 
    where TKey : IEquatable<TKey>
{
    [MaxLength(64)]
    public string RefreshToken { get; set; } = Guid.NewGuid().ToString();
    public DateTime Expiration  { get; set; } = DateTime.UtcNow.AddDays(7);
    

    [MaxLength(64)]
    public string? PreviousRefreshToken { get; set; } 
    public DateTime PreviousExpiration  { get; set; } = DateTime.UtcNow.AddDays(7);
}