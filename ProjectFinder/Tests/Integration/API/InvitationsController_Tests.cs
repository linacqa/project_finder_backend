using System.Net;
using System.Net.Http.Json;
using DTO.v1;
using DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Tests.Integration.API;

[Collection("Sequential")]
public class InvitationsController_Tests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private readonly ITestOutputHelper _testOutputHelper;

    public InvitationsController_Tests(CustomWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task GetInvitations_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.GetAsync("/api/v1/invitations");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetInvitations_With_Bearer_ReturnsOk()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.GetAsync("/api/v1/invitations");

        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<List<Invitation>>();
        Assert.NotNull(data);
    }

    [Fact]
    public async Task GetInvitation_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.GetAsync($"/api/v1/invitations/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetInvitation_With_Unknown_Id_ReturnsNotFound()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.GetAsync($"/api/v1/invitations/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetInvitationsByGroupId_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.GetAsync($"/api/v1/invitations/group/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetInvitationsByGroupId_With_Unknown_Group_ReturnsBadRequest()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.GetAsync($"/api/v1/invitations/group/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AcceptInvitation_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.PutAsync($"/api/v1/invitations/{Guid.NewGuid()}/accept", null);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AcceptInvitation_With_Unknown_Id_ReturnsNotFound()
    {
        ResetClientHeaders();
        await AuthorizeAsAdminUser();

        var response = await _client.PutAsync($"/api/v1/invitations/{Guid.NewGuid()}/accept", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeclineInvitation_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.PutAsync($"/api/v1/invitations/{Guid.NewGuid()}/decline", null);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeclineInvitation_With_Unknown_Id_ReturnsNotFound()
    {
        ResetClientHeaders();
        await AuthorizeAsAdminUser();

        var response = await _client.PutAsync($"/api/v1/invitations/{Guid.NewGuid()}/decline", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostInvitation_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var invitation = new InvitationCreate
        {
            GroupId = Guid.NewGuid(),
            ToUserId = Guid.NewGuid(),
            Role = "member"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/invitations", invitation);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PostInvitation_With_Unknown_Group_ReturnsBadRequest()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var invitation = new InvitationCreate
        {
            GroupId = Guid.NewGuid(),
            ToUserId = Guid.NewGuid(),
            Role = "member"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/invitations", invitation);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteInvitation_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.DeleteAsync($"/api/v1/invitations/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteInvitation_With_Unknown_Id_ReturnsNotFound()
    {
        ResetClientHeaders();
        await AuthorizeAsAdminUser();

        var response = await _client.DeleteAsync($"/api/v1/invitations/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private async Task AuthorizeAsDefaultUser()
    {
        var jwt = await LoginAndGetJwt();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
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
        string email = "user-a@test.ee",
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