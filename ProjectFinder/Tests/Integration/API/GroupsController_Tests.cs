using System.Net;
using System.Net.Http.Json;
using DTO.v1;
using DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Tests.Integration.API;

[Collection("Sequential")]
public class GroupsController_Tests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public GroupsController_Tests(CustomWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task GetGroups_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.GetAsync("/api/v1/groups");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetGroups_With_Student_Bearer_ReturnsOk()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.GetAsync("/api/v1/groups");

        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<List<Group>>();
        Assert.NotNull(data);
    }

    [Fact]
    public async Task GetGroups_With_Admin_Bearer_ReturnsOk()
    {
        ResetClientHeaders();
        await AuthorizeAsAdminUser();

        var response = await _client.GetAsync("/api/v1/groups");

        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<List<Group>>();
        Assert.NotNull(data);
    }

    [Fact]
    public async Task GetGroup_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.GetAsync($"/api/v1/groups/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetGroup_With_Unknown_Id_ReturnsNotFound()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.GetAsync($"/api/v1/groups/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task MatchingProjectTeamSize_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.GetAsync($"/api/v1/groups/matchingProjectTeamSize/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task MatchingProjectTeamSize_With_Unknown_Project_ReturnsNotFound()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.GetAsync($"/api/v1/groups/matchingProjectTeamSize/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostGroup_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var group = new GroupCreateUpdate
        {
            Name = $"test-group-{Guid.NewGuid()}",
            CreatorRoleInGroup = "owner"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/groups", group);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PostGroup_With_Student_Bearer_ReturnsCreated()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var group = new GroupCreateUpdate
        {
            Name = $"test-group-{Guid.NewGuid()}",
            CreatorRoleInGroup = "owner"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/groups", group);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdGroup = await response.Content.ReadFromJsonAsync<Group>();
        Assert.NotNull(createdGroup);
        Assert.Equal(group.Name, createdGroup!.Name);
    }

    [Fact]
    public async Task PostGroup_With_Missing_Name_ReturnsBadRequest()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var group = new GroupCreateUpdate
        {
            CreatorRoleInGroup = "owner"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/groups", group);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostGroup_With_Missing_CreatorRole_ReturnsBadRequest()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var group = new GroupCreateUpdate
        {
            Name = $"test-group-{Guid.NewGuid()}"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/groups", group);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteGroup_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.DeleteAsync($"/api/v1/groups/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteGroup_With_Unknown_Id_ReturnsNotFound()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.DeleteAsync($"/api/v1/groups/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteGroup_Created_By_Current_User_ReturnsNoContent()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var createdGroup = await CreateGroup();

        var response = await _client.DeleteAsync($"/api/v1/groups/{createdGroup.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteGroupMember_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.DeleteAsync($"/api/v1/groups/member/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteGroupMember_With_Unknown_Id_ReturnsNotFound()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.DeleteAsync($"/api/v1/groups/member/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private async Task<Group> CreateGroup()
    {
        var group = new GroupCreateUpdate
        {
            Name = $"test-group-{Guid.NewGuid()}",
            CreatorRoleInGroup = "owner"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/groups", group);
        response.EnsureSuccessStatusCode();

        var createdGroup = await response.Content.ReadFromJsonAsync<Group>();
        Assert.NotNull(createdGroup);

        return createdGroup!;
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

    private async Task<string> LoginAndGetJwt(
        string email,
        string password = "Test.Pass.1")
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