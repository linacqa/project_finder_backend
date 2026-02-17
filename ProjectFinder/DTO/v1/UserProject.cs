using Base.Contracts;

namespace DTO.v1;

public class UserProject : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid ProjectId { get; set; }
    
    public Guid UserProjectRoleId { get; set; }
}