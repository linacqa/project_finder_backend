using System.Net;
using System.Net.Http.Json;
using DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Tests.Integration.API;

[Collection("Sequential")]
public class UsersController_Tests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _testOutputHelper;

    public UsersController_Tests(CustomWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task GetUsers_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.GetAsync("/api/v1/users");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetUsers_With_Student_Bearer_ReturnsForbidden()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.GetAsync("/api/v1/users");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetUsers_With_Admin_Bearer_ReturnsOk()
    {
        ResetClientHeaders();
        await AuthorizeAsAdminUser();

        var response = await _client.GetAsync("/api/v1/users");
        
        var body = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine(body);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response.EnsureSuccessStatusCode();

        var users = await response.Content.ReadFromJsonAsync<List<UserInfo>>();
        Assert.NotNull(users);
        Assert.NotEmpty(users!);
    }

    [Fact]
    public async Task GetSupervisors_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.GetAsync("/api/v1/users/Supervisors");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetSupervisors_With_Bearer_ReturnsOk()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.GetAsync("/api/v1/users/Supervisors");

        response.EnsureSuccessStatusCode();

        var supervisors = await response.Content.ReadFromJsonAsync<List<SupervisorInfo>>();
        Assert.NotNull(supervisors);
    }

    [Fact]
    public async Task GetStudents_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.GetAsync("/api/v1/users/Students");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetStudents_With_Bearer_ReturnsOk()
    {
        ResetClientHeaders();
        await AuthorizeAsAdminUser();

        var response = await _client.GetAsync("/api/v1/users/Students");

        response.EnsureSuccessStatusCode();

        var students = await response.Content.ReadFromJsonAsync<List<StudentInfo>>();
        Assert.NotNull(students);
    }

    [Fact]
    public async Task GetUser_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.GetAsync($"/api/v1/users/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetUser_With_Unknown_Id_ReturnsNotFound()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.GetAsync($"/api/v1/users/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetUser_With_Existing_Id_ReturnsOk()
    {
        ResetClientHeaders();
        await AuthorizeAsAdminUser();

        var usersResponse = await _client.GetAsync("/api/v1/users");
        usersResponse.EnsureSuccessStatusCode();

        var users = await usersResponse.Content.ReadFromJsonAsync<List<UserInfo>>();
        Assert.NotNull(users);
        Assert.NotEmpty(users!);

        var response = await _client.GetAsync($"/api/v1/users/{users![0].Id}");

        response.EnsureSuccessStatusCode();

        var user = await response.Content.ReadFromJsonAsync<UserInfo>();
        Assert.NotNull(user);
        Assert.Equal(users[0].Id, user!.Id);
    }

    [Fact]
    public async Task EmailAdmins_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var request = new AdminEmailRequest
        {
            Subject = "Test subject",
            Body = "Test body"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/users/EmailAdmins", request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ChangeUserRole_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();

        var response = await _client.PatchAsync($"/api/v1/users/{Guid.NewGuid()}?role=student", null);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ChangeUserRole_With_Student_Bearer_ReturnsForbidden()
    {
        ResetClientHeaders();
        await AuthorizeAsStudentUser();

        var response = await _client.PatchAsync($"/api/v1/users/{Guid.NewGuid()}?role=teacher", null);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task ChangeUserRole_With_Admin_Unknown_User_ReturnsNotFound()
    {
        ResetClientHeaders();
        await AuthorizeAsAdminUser();

        var response = await _client.PatchAsync($"/api/v1/users/{Guid.NewGuid()}?role=teacher", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ChangeUserRole_With_Admin_Unknown_Role_ReturnsBadRequest()
    {
        ResetClientHeaders();
        await AuthorizeAsAdminUser();

        var student = await GetFirstStudent();

        var response = await _client.PatchAsync($"/api/v1/users/{student.Id}?role=unknown-role", null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ChangeUserRole_User_Already_Has_Role_ReturnsBadRequest()
    {
        ResetClientHeaders();
        await AuthorizeAsAdminUser();

        var student = await GetFirstStudent();

        var response = await _client.PatchAsync($"/api/v1/users/{student.Id}?role=student", null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private async Task<StudentInfo> GetFirstStudent()
    {
        var response = await _client.GetAsync("/api/v1/users/Students");
        response.EnsureSuccessStatusCode();

        var students = await response.Content.ReadFromJsonAsync<List<StudentInfo>>();
        Assert.NotNull(students);
        Assert.NotEmpty(students!);

        return students![0];
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