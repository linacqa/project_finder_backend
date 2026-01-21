using Base.Contracts;

namespace DTO.v1;

public class UserGroup : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid GroupId { get; set; }
}