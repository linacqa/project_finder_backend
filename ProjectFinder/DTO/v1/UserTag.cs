using Base.Contracts;

namespace DTO.v1;

public class UserTag : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid TagId { get; set; }
}