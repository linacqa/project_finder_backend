using Base.Domain;

namespace Domain;

public class GroupProject : BaseEntity
{
    public Guid GroupId { get; set; }
    public Group? Group { get; set; }

    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
}