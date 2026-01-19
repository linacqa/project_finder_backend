using Base.Contracts;

namespace BLL.DTO;

public class GroupProject : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid GroupId { get; set; }
    public Group? Group { get; set; }

    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
}