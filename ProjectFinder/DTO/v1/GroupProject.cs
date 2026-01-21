using Base.Contracts;

namespace DTO.v1;

public class GroupProject : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid GroupId { get; set; }

    public Guid ProjectId { get; set; }
}