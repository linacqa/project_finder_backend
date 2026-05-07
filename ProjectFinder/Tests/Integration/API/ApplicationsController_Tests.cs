using System.Net;
using System.Net.Http.Json;
using DTO.v1;
using DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Tests.Integration.API;

[Collection("Sequential")]
public class ApplicationsController_Tests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ApplicationsController_Tests(CustomWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task GetApplications_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.GetAsync("/api/v1/applications");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetApplications_With_Student_Bearer_ReturnsForbidden()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.GetAsync("/api/v1/applications");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetApplications_With_Admin_Bearer_ReturnsOk()
    {
        ResetClientHeaders();
        await AuthorizeAsAdminUser();

        var response = await _client.GetAsync("/api/v1/applications");

        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<List<Application>>();
        Assert.NotNull(data);
    }

    [Fact]
    public async Task GetMyApplications_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.GetAsync("/api/v1/applications/my");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetMyApplications_With_Student_Bearer_ReturnsOk()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.GetAsync("/api/v1/applications/my");

        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<List<Application>>();
        Assert.NotNull(data);
    }

    [Fact]
    public async Task GetApplication_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.GetAsync($"/api/v1/applications/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetApplication_With_Unknown_Id_ReturnsNotFound()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.GetAsync($"/api/v1/applications/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetMyApplicationByProjectId_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.GetAsync($"/api/v1/applications/my/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetMyApplicationByProjectId_With_Unknown_Project_ReturnsNotFound()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.GetAsync($"/api/v1/applications/my/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostApplication_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var application = new ApplicationCreate
        {
            ProjectId = Guid.NewGuid()
        };

        var response = await _client.PostAsJsonAsync("/api/v1/applications", application);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PostApplication_With_Unknown_Project_ReturnsBadRequest()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var application = new ApplicationCreate
        {
            ProjectId = Guid.NewGuid()
        };

        var response = await _client.PostAsJsonAsync("/api/v1/applications", application);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AcceptApplication_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.PutAsync($"/api/v1/applications/{Guid.NewGuid()}/accept", null);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AcceptApplication_With_Student_Bearer_ReturnsForbidden()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.PutAsync($"/api/v1/applications/{Guid.NewGuid()}/accept", null);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task AcceptApplication_With_Admin_Unknown_Id_ReturnsNotFound()
    {
        ResetClientHeaders();
        await AuthorizeAsAdminUser();

        var response = await _client.PutAsync($"/api/v1/applications/{Guid.NewGuid()}/accept", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeclineApplication_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.PutAsync($"/api/v1/applications/{Guid.NewGuid()}/decline", null);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeclineApplication_With_Student_Bearer_ReturnsForbidden()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.PutAsync($"/api/v1/applications/{Guid.NewGuid()}/decline", null);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task DeclineApplication_With_Admin_Unknown_Id_ReturnsNotFound()
    {
        ResetClientHeaders();
        await AuthorizeAsAdminUser();

        var response = await _client.PutAsync($"/api/v1/applications/{Guid.NewGuid()}/decline", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteApplication_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.DeleteAsync($"/api/v1/applications/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteApplication_With_Unknown_Id_ReturnsNotFound()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.DeleteAsync($"/api/v1/applications/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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