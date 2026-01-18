using Base.Contracts;
using DAL.DTO.Identity;

namespace DAL.DTO;

public class UserGroup : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    
    public Guid GroupId { get; set; }
    public Group? Group { get; set; }
}