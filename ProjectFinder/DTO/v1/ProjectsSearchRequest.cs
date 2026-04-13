using System.ComponentModel.DataAnnotations;

namespace DTO.v1;

public class ProjectsSearchRequest
{
    [MaxLength(64)]
    public string? Title { get; set; }
    public int? MinStudents { get; set; }
    public int? MaxStudents { get; set; }
    public List<Guid>? TagIds { get; set; }
    public List<Guid>? StatusIds { get; set; }
    public List<Guid>? ProjectTypeIds { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}