using System.Net;
using System.Net.Http.Json;
using DTO.v1;
using DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Tests.Helpers;
using Xunit.Abstractions;

namespace Tests.Integration.API;

[Collection("Sequential")]
public class ProjectsController_Tests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProjectsController_Tests(CustomWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task GetProjects_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.GetAsync("/api/v1/projects");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetProjects_With_Bearer_ReturnsOk()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.GetAsync("/api/v1/projects");

        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<List<Project>>();
        Assert.NotNull(data);
    }

    [Fact]
    public async Task SearchProjects_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.GetAsync("/api/v1/projects/search");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task SearchProjects_With_Bearer_ReturnsOk()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.GetAsync("/api/v1/projects/search?page=1&pageSize=10");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetCurrentUsersProjects_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.GetAsync("/api/v1/projects/my");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetCurrentUsersProjects_With_Bearer_ReturnsOk()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.GetAsync("/api/v1/projects/my");

        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<List<Project>>();
        Assert.NotNull(data);
    }

    [Fact]
    public async Task GetProject_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.GetAsync($"/api/v1/projects/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetProject_With_Unknown_Id_ReturnsNotFound()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.GetAsync($"/api/v1/projects/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostProject_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var project = CreateProjectCreate();

        var response = await _client.PostAsJsonAsync("/api/v1/projects", project);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PostProject_With_Student_Bearer_ReturnsForbidden()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var project = CreateProjectCreate();

        var response = await _client.PostAsJsonAsync("/api/v1/projects", project);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task PostProject_With_Admin_Bearer_ReturnsCreated()
    {
        ResetClientHeaders();
        await AuthorizeAsAdminUser();

        var project = CreateProjectCreate();

        var response = await _client.PostAsJsonAsync("/api/v1/projects", project);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdProject = await response.Content.ReadFromJsonAsync<Project>();
        Assert.NotNull(createdProject);
        Assert.Equal(project.TitleInEstonian, createdProject!.TitleInEstonian);
    }

    [Fact]
    public async Task PutProject_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.PutAsJsonAsync($"/api/v1/projects/{Guid.NewGuid()}", CreateProjectCreate());

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PutProject_With_Student_Bearer_ReturnsForbidden()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.PutAsJsonAsync($"/api/v1/projects/{Guid.NewGuid()}", CreateProjectCreate());

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task PutProject_With_Admin_Unknown_Id_ReturnsBadRequest()
    {
        ResetClientHeaders();
        await AuthorizeAsAdminUser();

        var response = await _client.PutAsJsonAsync($"/api/v1/projects/{Guid.NewGuid()}", CreateProjectCreate());

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteProject_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.DeleteAsync($"/api/v1/projects/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteProject_With_Student_Bearer_ReturnsForbidden()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.DeleteAsync($"/api/v1/projects/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task DeleteProject_With_Admin_Unknown_Id_ReturnsNotFound()
    {
        ResetClientHeaders();
        await AuthorizeAsAdminUser();

        var response = await _client.DeleteAsync($"/api/v1/projects/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteProject_Created_By_Admin_ReturnsNoContent()
    {
        ResetClientHeaders();
        await AuthorizeAsAdminUser();

        var createdProject = await CreateProject();

        var response = await _client.DeleteAsync($"/api/v1/projects/{createdProject.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    private static ProjectCreate CreateProjectCreate()
    {
        return new ProjectCreate
        {
            TitleInEstonian = $"test-project-{Guid.NewGuid()}",
            Description = "Integration test project",
            MinStudents = 1,
            MaxStudents = 3,
            ProjectTypeId = TestUsers.ProjectTypeAId,
            ProjectStatusId = TestUsers.ProjectStatusAId,
            AuthorId = TestUsers.AdminUserId,
        };
    }

    private async Task<Project> CreateProject()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/projects", CreateProjectCreate());
        response.EnsureSuccessStatusCode();

        var project = await response.Content.ReadFromJsonAsync<Project>();
        Assert.NotNull(project);

        return project!;
    }

    private async Task AuthorizeAsStudentUser()
    {
        var jwt = await LoginAndGetJwt("student-a@test.ee");

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
    }

    private async Task AuthorizeAsAdminUser()
    {
        var jwt = await LoginAndGetJwt("admin@test.ee");

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
    }

    private async Task<string> LoginAndGetJwt(string email, string password = "Test.Pass.1")
    {
        var response = await _client.PostAsJsonAsync("/api/v1/account/login", new LoginInfo
        {
            Email = email,
            Password = password
        });

        response.EnsureSuccessStatusCode();

        var jwt = GetCookieValue(response.Headers.GetValues("Set-Cookie"), "accessToken");
        Assert.False(string.IsNullOrWhiteSpace(jwt));

        return jwt!;
    }

    private static string? GetCookieValue(IEnumerable<string> setCookieHeaders, string cookieName)
    {
        foreach (var header in setCookieHeaders)
        {
            var firstPart = header.Split(';')[0].Trim();
            var kv = firstPart.Split(new[] { '=' }, 2);

            if (kv.Length == 2 && kv[0].Trim() == cookieName)
            {
                return kv[1];
            }
        }

        return null;
    }

    private void ResetClientHeaders()
    {
        _client.DefaultRequestHeaders.Authorization = null;

        if (_client.DefaultRequestHeaders.Contains("Cookie"))
        {
            _client.DefaultRequestHeaders.Remove("Cookie");
        }
    }
}