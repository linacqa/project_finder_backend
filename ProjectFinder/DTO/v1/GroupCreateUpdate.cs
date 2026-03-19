namespace DTO.v1;

public class GroupCreateUpdate
{
    public string Name { get; set; } = default!;
    
    public string? CreatorRoleInGroup { get; set; } = default!;
}