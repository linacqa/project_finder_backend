namespace DTO.v1;

public class ProjectsSearchRequest
{
    public string? Title { get; set; }
    public int? MinStudents { get; set; }
    public int? MaxStudents { get; set; }
    public List<Guid>? TagIds { get; set; }
    public List<Guid>? StatusIds { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}