using Base.Contracts;
using BLL.DTO.Identity;

namespace BLL.DTO;

public class UserTag : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    
    public Guid TagId { get; set; }
    public Tag? Tag { get; set; }
}