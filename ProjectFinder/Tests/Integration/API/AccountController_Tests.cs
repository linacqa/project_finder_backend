using System.Net;
using System.Net.Http.Json;
using DTO.v1;
using DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Tests.Integration.API;

[Collection("Sequential")]
public class AccountController_Tests: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private readonly ITestOutputHelper _testOutputHelper;


    public AccountController_Tests(CustomWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
    
    [Fact]
    public async Task Register_New_User()
    {
        ResetClientHeaders();
        // Arrange
        var registrationData = new RegisterInfo()
        {
            Email = "test@test.ee",
            FirstName = "Test",
            LastName = "User",
            Password = "Password.123",
            Role = "user"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/account/register", registrationData);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseData = await response.Content.ReadFromJsonAsync<Message>();
        Assert.NotNull(responseData);
        var cookies = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
        var jwt = GetJWTOrRefreshTokenFromCookies(cookies);
        Assert.True(jwt?.Length > 128);
        // Assert.True(responseData.RefreshToken.Length == Guid.NewGuid().ToString().Length);
    }

    [Fact]
    public async Task Login_Existing_User()
    {
        ResetClientHeaders();
        // Arrange
        var loginData = new LoginInfo()
        {
            Email = "user-a@test.ee",
            Password = "Test.Pass.1"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/account/login", loginData);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseData = await response.Content.ReadFromJsonAsync<JWTResponse>();
        Assert.NotNull(responseData);
        var cookies = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
        var jwt = GetJWTOrRefreshTokenFromCookies(cookies);
        Assert.True(jwt?.Length > 128);
        // Assert.True(responseData.RefreshToken.Length == Guid.NewGuid().ToString().Length);
    }

    [Fact]
    public async Task Login_Existing_User_Check_Rights()
    {
        ResetClientHeaders();
        // Arrange
        var loginData = new LoginInfo()
        {
            Email = "user-a@test.ee",
            Password = "Test.Pass.1"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/account/login", loginData);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseData = await response.Content.ReadFromJsonAsync<JWTResponse>();
        Assert.NotNull(responseData);
        var cookies = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
        var jwt = GetJWTOrRefreshTokenFromCookies(cookies);
        Assert.True(jwt?.Length > 128);
        // Assert.True(responseData.RefreshToken.Length == Guid.NewGuid().ToString().Length);
        

        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
        
        var getResponse = await _client.GetAsync("/api/v1/account/currentUserInfo");
        getResponse.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task No_Bearer_Header_Unauthorized()
    {
        ResetClientHeaders();
        var getResponse = await _client.GetAsync("/api/v1/account/currentUserInfo");
        Assert.Equal(HttpStatusCode.Unauthorized, getResponse.StatusCode);
    }
    
    [Fact]
    public async Task JWT_Custom_Expiration()
    {
        ResetClientHeaders();
        // Arrange
        var loginData = new LoginInfo()
        {
            Email = "user-a@test.ee",
            Password = "Test.Pass.1"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/account/login?jwtExpiresInSeconds=1", loginData);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseData = await response.Content.ReadFromJsonAsync<JWTResponse>();
        Assert.NotNull(responseData);
        var cookies = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
        var jwt = GetJWTOrRefreshTokenFromCookies(cookies);
        Assert.True(jwt?.Length > 128);
        // Assert.True(responseData.RefreshToken.Length == Guid.NewGuid().ToString().Length);
        

        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
        
        var getResponse = await _client.GetAsync("/api/v1/account/currentUserInfo");
        getResponse.EnsureSuccessStatusCode();


        // Wait for JWT to expire
        await Task.Delay(1500);
        var getResponseAuthExpired = await _client.GetAsync("/api/v1/account/currentUserInfo");
        
        Assert.Equal(HttpStatusCode.Unauthorized, getResponseAuthExpired.StatusCode);
    }

    [Fact]
    public async Task JWT_Refresh()
    {
        ResetClientHeaders();
        // Arrange
        var loginData = new LoginInfo()
        {
            Email = "user-a@test.ee",
            Password = "Test.Pass.1"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/account/login?jwtExpiresInSeconds=1", loginData);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseData = await response.Content.ReadFromJsonAsync<JWTResponse>();
        Assert.NotNull(responseData);
        var setCookieHeaders = response.Headers.GetValues("Set-Cookie");
        var jwt = GetCookieValue(setCookieHeaders, "accessToken");
        var refreshToken = GetCookieValue(setCookieHeaders, "refreshToken");
        Assert.True(jwt?.Length > 128);
        // Assert.True(responseData.RefreshToken.Length == Guid.NewGuid().ToString().Length);
        

        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
        // send cookies in the request using the Cookie header
        if (_client.DefaultRequestHeaders.Contains("Cookie")) _client.DefaultRequestHeaders.Remove("Cookie");
        _client.DefaultRequestHeaders.Add("Cookie", $"accessToken={jwt}; refreshToken={refreshToken}");
        
        var getResponse = await _client.GetAsync("/api/v1/account/currentUserInfo");
        getResponse.EnsureSuccessStatusCode();


        // Wait for JWT to expire
        await Task.Delay(1500);

        var getResponseAuthExpired = await _client.GetAsync("/api/v1/account/currentUserInfo");
        
        Assert.Equal(HttpStatusCode.Unauthorized, getResponseAuthExpired.StatusCode);
        
        // Refresh JWT
        // Call renew endpoint; server will read cookies from the request
        var refreshResponse = await _client.PostAsync("/api/v1/account/RenewRefreshToken", null);

        var refreshedResponseData = await refreshResponse.Content.ReadFromJsonAsync<JWTResponse>();
        Assert.NotNull(refreshedResponseData);
        
        var newSetCookie = refreshResponse.Headers.GetValues("Set-Cookie");
        jwt = GetCookieValue(newSetCookie, "accessToken");
        refreshToken = GetCookieValue(newSetCookie, "refreshToken");
        if (_client.DefaultRequestHeaders.Contains("Cookie")) _client.DefaultRequestHeaders.Remove("Cookie");
        _client.DefaultRequestHeaders.Add("Cookie", $"accessToken={jwt}; refreshToken={refreshToken}");

        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
        
        var getResponse2 = await _client.GetAsync("/api/v1/account/currentUserInfo");
        getResponse2.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Logout_User()
    {
        ResetClientHeaders();
        // Arrange
        var loginData = new LoginInfo()
        {
            Email = "user-a@test.ee",
            Password = "Test.Pass.1"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/account/login", loginData);
        
        var responseData = await response.Content.ReadFromJsonAsync<JWTResponse>();
        
        var setCookieHeaders = response.Headers.GetValues("Set-Cookie");
        var jwt = GetCookieValue(setCookieHeaders, "accessToken");
        var refreshToken = GetCookieValue(setCookieHeaders, "refreshToken");
        
        if (_client.DefaultRequestHeaders.Contains("Cookie")) _client.DefaultRequestHeaders.Remove("Cookie");
        _client.DefaultRequestHeaders.Add("Cookie", $"accessToken={jwt}; refreshToken={refreshToken}");
        
        var logoutResponse = await _client.PostAsJsonAsync("/api/v1/account/logout", new LogoutInfo()
        {
            RefreshToken = responseData?.RefreshToken ?? string.Empty
        });

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(responseData);
        
        Assert.True(jwt?.Length > 128);
        Assert.True(refreshToken?.Length == Guid.NewGuid().ToString().Length);
        
        logoutResponse.EnsureSuccessStatusCode();
        
        if (_client.DefaultRequestHeaders.Contains("Cookie")) _client.DefaultRequestHeaders.Remove("Cookie");
        
        var getResponseAfterLogout = await _client.GetAsync("/api/v1/account/currentUserInfo");
        Assert.Equal(HttpStatusCode.Unauthorized, getResponseAfterLogout.StatusCode);
    }
    
    [Fact]
    public async Task Register_Duplicate_Email_ReturnsBadRequest()
    {
        ResetClientHeaders();
        var registrationData = new RegisterInfo
        {
            Email = "user-a@test.ee",
            FirstName = "Test",
            LastName = "User",
            Password = "Password.123",
            Role = "user"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/account/register", registrationData);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_Invalid_Role_ReturnsBadRequest()
    {
        ResetClientHeaders();
        var registrationData = new RegisterInfo
        {
            Email = $"new-{Guid.NewGuid()}@test.ee",
            FirstName = "Test",
            LastName = "User",
            Password = "Password.123",
            Role = "admin"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/account/register", registrationData);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_Weak_Password_ReturnsBadRequest()
    {
        ResetClientHeaders();
        var registrationData = new RegisterInfo
        {
            Email = $"weak-{Guid.NewGuid()}@test.ee",
            FirstName = "Test",
            LastName = "User",
            Password = "123",
            Role = "user"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/account/register", registrationData);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_NonExisting_User_ReturnsNotFound()
    {
        ResetClientHeaders();
        var loginData = new LoginInfo
        {
            Email = $"missing-{Guid.NewGuid()}@test.ee",
            Password = "Test.Pass.1"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/account/login", loginData);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Login_Wrong_Password_ReturnsNotFound()
    {
        ResetClientHeaders();
        var loginData = new LoginInfo
        {
            Email = "user-a@test.ee",
            Password = "Wrong.Pass.1"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/account/login", loginData);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task RenewRefreshToken_Without_Cookies_ReturnsBadRequest()
    {
        ResetClientHeaders();
        if (_client.DefaultRequestHeaders.Contains("Cookie"))
            _client.DefaultRequestHeaders.Remove("Cookie");

        var response = await _client.PostAsync("/api/v1/account/RenewRefreshToken", null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task RenewRefreshToken_With_Invalid_Jwt_ReturnsBadRequest()
    {
        ResetClientHeaders();
        if (_client.DefaultRequestHeaders.Contains("Cookie"))
            _client.DefaultRequestHeaders.Remove("Cookie");

        _client.DefaultRequestHeaders.Add("Cookie", "accessToken=not-a-jwt; refreshToken=not-a-refresh-token");

        var response = await _client.PostAsync("/api/v1/account/RenewRefreshToken", null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task RenewRefreshToken_With_Invalid_RefreshToken_ReturnsServerError()
    {
        ResetClientHeaders();
        var jwt = await LoginAndGetJwt();

        if (_client.DefaultRequestHeaders.Contains("Cookie"))
            _client.DefaultRequestHeaders.Remove("Cookie");

        _client.DefaultRequestHeaders.Add("Cookie", $"accessToken={jwt}; refreshToken={Guid.NewGuid()}");

        var response = await _client.PostAsync("/api/v1/account/RenewRefreshToken", null);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCurrentUserInfo_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();
        var model = new UpdateAccountInfo
        {
            FirstName = "Updated",
            LastName = "User",
            Email = "user-a@example.com"
        };

        var response = await _client.PutAsJsonAsync("/api/v1/account/UpdateCurrentUserInfo", model);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCurrentUserInfo_Updates_Basic_User_Fields()
    {
        ResetClientHeaders();
        var email = $"update-{Guid.NewGuid()}@test.ee";
        var jwt = await RegisterAndGetJwt(email);
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);

        var model = new UpdateAccountInfo
        {
            FirstName = "UpdatedFirst",
            LastName = "UpdatedLast",
            Email = email,
            PhoneNumber = "+37255555555"
        };

        var response = await _client.PutAsJsonAsync("/api/v1/account/UpdateCurrentUserInfo", model);

        response.EnsureSuccessStatusCode();

        var userInfo = await response.Content.ReadFromJsonAsync<CurrentUserInfo>();
        Assert.NotNull(userInfo);
        Assert.Equal("UpdatedFirst", userInfo!.FirstName);
        Assert.Equal("UpdatedLast", userInfo.LastName);
        Assert.Equal("+37255555555", userInfo.PhoneNumber);
    }

    [Fact]
    public async Task UpdateCurrentUserInfo_Email_Already_In_Use_ReturnsBadRequest()
    {
        ResetClientHeaders();
        var jwt = await LoginAndGetJwt();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);

        var model = new UpdateAccountInfo
        {
            Email = "user-b@test.ee"
        };

        var response = await _client.PutAsJsonAsync("/api/v1/account/UpdateCurrentUserInfo", model);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Without_Bearer_ReturnsUnauthorized()
    {
        ResetClientHeaders();
        var response = await _client.DeleteAsync("/api/v1/account/Delete");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Authenticated_User_Anonymizes_And_Disables_Login()
    {
        ResetClientHeaders();
        var email = $"delete-{Guid.NewGuid()}@test.ee";
        var password = "Test.Pass.1";
        
        var jwt = await RegisterAndGetJwt(email, password);

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);

        var deleteResponse = await _client.DeleteAsync("/api/v1/account/Delete");
        deleteResponse.EnsureSuccessStatusCode();

        _client.DefaultRequestHeaders.Authorization = null;

        var loginResponse = await _client.PostAsJsonAsync("/api/v1/account/login", new LoginInfo
        {
            Email = email,
            Password = password
        });

        Assert.Equal(HttpStatusCode.NotFound, loginResponse.StatusCode);
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
    
    private async Task<string> RegisterAndGetJwt(string email, string password = "Password.123")
    {
        var response = await _client.PostAsJsonAsync("/api/v1/account/register", new RegisterInfo
        {
            Email = email,
            FirstName = "Test",
            LastName = "User",
            Password = password,
            Role = "user"
        });

        response.EnsureSuccessStatusCode();

        var jwt = GetCookieValue(response.Headers.GetValues("Set-Cookie"), "accessToken");
        Assert.False(string.IsNullOrWhiteSpace(jwt));

        return jwt!;
    }

    private string? GetJWTOrRefreshTokenFromCookies(string? cookies)
    {
        return cookies?.Split(';')[0].Split('=')[1];
    }

    private static string? GetCookieValue(IEnumerable<string> setCookieHeaders, string cookieName)
    {
        if (setCookieHeaders == null) return null;
        foreach (var header in setCookieHeaders)
        {
            if (string.IsNullOrEmpty(header)) continue;
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
