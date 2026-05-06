using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using Base.Domain.Identity;
using Base.Helpers;
using DAL.EF;
using DAL.EF.DataSeeding;
using Domain.Identity;
using DTO.v1;
using DTO.v1.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.Identity;

/// <summary>
/// User account controller - login, register, etc.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly IConfiguration _configuration;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly Random _random = new Random();
    private readonly AppDbContext _context;

    private const string UserPassProblem = "User/Password problem";
    private const int RandomDelayMin = 500;
    private const int RandomDelayMax = 5000;

    private const string SettingsJWTPrefix = "JWTSecurity";
    private const string SettingsJWTKey = SettingsJWTPrefix + ":Key";
    private const string SettingsJWTIssuer = SettingsJWTPrefix + ":Issuer";
    private const string SettingsJWTAudience = SettingsJWTPrefix + ":Audience";
    private const string SettingsJWTExpiresInSeconds = SettingsJWTPrefix + ":ExpiresInSeconds";
    private const string SettingsJWTRefreshTokenExpiresInSeconds = SettingsJWTPrefix + ":RefreshTokenExpiresInSeconds";


    /// <summary>
    /// Constructor
    /// </summary>
    public AccountController(IConfiguration configuration, UserManager<AppUser> userManager,
        ILogger<AccountController> logger, SignInManager<AppUser> signInManager, AppDbContext context)
    {
        _configuration = configuration;
        _userManager = userManager;
        _logger = logger;
        _signInManager = signInManager;
        _context = context;
    }

    /// <summary>
    /// Register new user
    /// </summary>
    /// <param name="registrationData">Registration info</param>
    /// <param name="jwtExpiresInSeconds">Optional custom jwt expiration</param>
    /// <param name="refreshTokenExpiresInSeconds">Optional custom refresh token expiration</param>
    /// <returns></returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Message), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Message), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<Message>> Register(
        [FromBody] RegisterInfo registrationData,
        [FromQuery] int? jwtExpiresInSeconds,
        [FromQuery] int? refreshTokenExpiresInSeconds)
    {
        // is user already registered
        var appUser = await _userManager.FindByEmailAsync(registrationData.Email);
        if (appUser != null)
        {
            _logger.LogWarning("User with email {} is already registered", registrationData.Email);
            return BadRequest(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = $"User with email {registrationData.Email} is already registered"
                }
            );
        }
        
        var availableRoles = new List<string>() { "user", "student", "teacher" };
        
        if (!availableRoles.Contains(registrationData.Role.ToLower()))
        {
            return BadRequest(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = $"Role {registrationData.Role} is unavailable"
                }
            );
        }

        // register user
        var refreshToken = new AppRefreshToken()
        {
            Expiration = GetExpirationDateTime(refreshTokenExpiresInSeconds, SettingsJWTRefreshTokenExpiresInSeconds)
        };

        appUser = new AppUser()
        {
            Email = registrationData.Email,
            PhoneNumber = registrationData.PhoneNumber,
            UserName = registrationData.Email,
            FirstName = registrationData.FirstName,
            LastName = registrationData.LastName,
            UniId = registrationData.UniId,
            MatriculationNumber = registrationData.MatriculationNumber,
            Program = registrationData.Program,
            AuthType = AuthType.Local,
            RefreshTokens = new List<AppRefreshToken>() { refreshToken }
        };
        refreshToken.User = appUser;

        var result = await _userManager.CreateAsync(appUser, registrationData.Password);

        if (!result.Succeeded)
        {
            return BadRequest(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = result.Errors.First().Description
                }
            );
        }
        
        result = await _userManager.AddToRoleAsync(appUser, registrationData.Role);
        if (!result.Succeeded)
        {
            return BadRequest(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = result.Errors.First().Description
                }
            );
        }

        // save into claims also the user full name
        result = await _userManager.AddClaimsAsync(appUser, new List<Claim>()
        {
            new(ClaimTypes.GivenName, appUser.FirstName),
            new(ClaimTypes.Surname, appUser.LastName),
        });

        // get full user from system with fixed data (maybe there is something generated by identity that we might need
        appUser = await _userManager.FindByEmailAsync(appUser.Email);
        if (appUser == null)
        {
            _logger.LogWarning("User with email {} is not found after registration", registrationData.Email);
            return BadRequest(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = $"User with email {registrationData.Email} is not found after registration"
                }
            );
        }

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration.GetValue<string>(SettingsJWTKey)!,
            _configuration.GetValue<string>(SettingsJWTIssuer)!,
            _configuration.GetValue<string>(SettingsJWTAudience)!,
            GetExpirationDateTime(jwtExpiresInSeconds, SettingsJWTExpiresInSeconds)
        );
        
        SetAuthCookies(jwt, refreshToken.RefreshToken);
        
        // var res = new JWTResponse()
        // {
        //     JWT = jwt,
        //     RefreshToken = refreshToken.RefreshToken,
        // };
        // return Ok(res);
        return Ok(new Message(["Registered and logged in"]));
    }

    /// <summary>
    /// User authentication
    /// </summary>
    /// <param name="loginInfo">Login model</param>
    /// <param name="jwtExpiresInSeconds">Optional, use custom jwt expiration</param>
    /// <param name="refreshTokenExpiresInSeconds">Optional, use custom refresh token expiration</param>
    /// <returns></returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Message), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Message), StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<ActionResult<Message>> Login(
        [FromBody] LoginInfo loginInfo,
        [FromQuery] int? jwtExpiresInSeconds,
        [FromQuery] int? refreshTokenExpiresInSeconds
    )
    {
        // verify user
        var appUser = await _userManager.FindByEmailAsync(loginInfo.Email);
        if (appUser == null)
        {
            _logger.LogWarning("WebApi login failed, email {} not found", loginInfo.Email);
            await Task.Delay(_random.Next(RandomDelayMin, RandomDelayMax));
            return NotFound(new Message(UserPassProblem));
        }

        // verify password
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginInfo.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("WebApi login failed, password {} for email {} was wrong", loginInfo.Password,
                loginInfo.Email);
            await Task.Delay(_random.Next(RandomDelayMin, RandomDelayMax));
            return NotFound(new Message(UserPassProblem));
        }

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (!_context.Database.ProviderName!.Contains("InMemory"))
        {
            var deletedRows = await _context
                .RefreshTokens
                .Where(t => t.UserId == appUser.Id && t.Expiration < DateTime.UtcNow)
                .ExecuteDeleteAsync();
            _logger.LogInformation("Deleted {} refresh tokens", deletedRows);
        }

        var refreshToken = new AppRefreshToken()
        {
            UserId = appUser.Id,
            Expiration = GetExpirationDateTime(refreshTokenExpiresInSeconds, SettingsJWTRefreshTokenExpiresInSeconds)
        };
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();


        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration.GetValue<string>(SettingsJWTKey)!,
            _configuration.GetValue<string>(SettingsJWTIssuer)!,
            _configuration.GetValue<string>(SettingsJWTAudience)!,
            GetExpirationDateTime(jwtExpiresInSeconds, SettingsJWTExpiresInSeconds)
        );
        
        SetAuthCookies(jwt, refreshToken.RefreshToken);

        // var responseData = new JWTResponse()
        // {
        //     JWT = jwt,
        //     RefreshToken = refreshToken.RefreshToken
        // };
        //
        // return Ok(responseData);
        return Ok(new Message(["Logged in"]));
    }

    // /// <summary>
    // /// User authentication via Azure, returns JWT and refresh token
    // /// </summary>
    // /// <param name="loginInfo">Azure login model</param>
    // /// <param name="jwtExpiresInSeconds">Optional, use custom jwt expiration</param>
    // /// <param name="refreshTokenExpiresInSeconds">Optional, use custom refresh token expiration</param>
    // /// <returns>JWT and refresh token</returns>
    // [HttpPost]
    // [ProducesResponseType(typeof(JWTResponse), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(Message), StatusCodes.Status400BadRequest)]
    // public async Task<ActionResult<JWTResponse>> LoginWithAzure(
    //     [FromBody] AzureLoginInfo loginInfo,
    //     [FromQuery] int? jwtExpiresInSeconds,
    //     [FromQuery] int? refreshTokenExpiresInSeconds
    // )
    // {
    //     // Validate Azure JWT
    //     var handler = new JwtSecurityTokenHandler();
    //     JwtSecurityToken azureJwt;
    //
    //     try
    //     {
    //         azureJwt = handler.ReadJwtToken(loginInfo.AccessToken);
    //     }
    //     catch
    //     {
    //         return BadRequest(new Message("Invalid Azure token"));
    //     }
    //
    //     // Get claims from Azure JWT
    //     var azureObjectId = azureJwt.Claims.FirstOrDefault(c => c.Type == "oid")?.Value;
    //     var email = azureJwt.Claims.FirstOrDefault(c =>
    //         c.Type == ClaimTypes.Email || c.Type == "preferred_username")?.Value;
    //     var fullName = azureJwt.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
    //
    //     if (azureObjectId == null || email == null)
    //     {
    //         return BadRequest(new Message("Invalid Azure claims"));
    //     }
    //
    //     // Search for existing user by Azure Object ID
    //     var appUser = await _context.Users
    //         .Include(u => u.RefreshTokens)
    //         .FirstOrDefaultAsync(u => u.AzureObjectId == azureObjectId);
    //
    //     // If not found, create new user
    //     if (appUser == null)
    //     {
    //         appUser = new AppUser
    //         {
    //             Email = email,
    //             UserName = email,
    //             FirstName = fullName?.Split(' ').FirstOrDefault(),
    //             LastName = fullName?.Split(' ').Skip(1).FirstOrDefault(),
    //             AzureObjectId = azureObjectId,
    //             AuthType = AuthType.AzureAD
    //         };
    //
    //         await _userManager.CreateAsync(appUser);
    //     }
    //
    //     // Refresh token
    //     var refreshToken = new AppRefreshToken
    //     {
    //         UserId = appUser.Id,
    //         Expiration = GetExpirationDateTime(
    //             refreshTokenExpiresInSeconds,
    //             SettingsJWTRefreshTokenExpiresInSeconds)
    //     };
    //
    //     _context.RefreshTokens.Add(refreshToken);
    //     await _context.SaveChangesAsync();
    //
    //     // Generate JWT
    //     var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
    //
    //     var jwt = IdentityExtensions.GenerateJwt(
    //         claimsPrincipal.Claims,
    //         _configuration.GetValue<string>(SettingsJWTKey)!,
    //         _configuration.GetValue<string>(SettingsJWTIssuer)!,
    //         _configuration.GetValue<string>(SettingsJWTAudience)!,
    //         GetExpirationDateTime(jwtExpiresInSeconds, SettingsJWTExpiresInSeconds)
    //     );
    //
    //     return Ok(new JWTResponse
    //     {
    //         JWT = jwt,
    //         RefreshToken = refreshToken.RefreshToken
    //     });
    // }

    /// <summary>
    /// Renew JWT using refresh token
    /// </summary>
    /// <param name="jwtExpiresInSeconds">Optional custom expiration for jwt</param>
    /// <param name="refreshTokenExpiresInSeconds">Optional custom expiration for refresh token</param>
    /// <returns></returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Message), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Message), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<Message>> RenewRefreshToken(
        // [FromBody] TokenRefreshInfo tokenRefreshInfo,
        [FromQuery] int? jwtExpiresInSeconds,
        [FromQuery] int? refreshTokenExpiresInSeconds
    )
    {
        var oldJWT = Request.Cookies["accessToken"];
        var oldRefreshToken = Request.Cookies["refreshToken"];

        if (oldJWT == null || oldRefreshToken == null)
        {
            return BadRequest(new Message("No token"));
        }

        JwtSecurityToken jwtToken;
        // get user info from jwt
        try
        {
            jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(oldJWT);
            if (jwtToken == null)
            {
                return BadRequest(new Message("No token"));
            }
        }
        catch (Exception e)
        {
            return BadRequest(new Message($"Cant parse the token, {e.Message}"));
        }

        // validate jwt, ignore expiration date
        if (!IdentityExtensions.ValidateJWT(
                oldJWT,
                _configuration.GetValue<string>(SettingsJWTKey)!,
                _configuration.GetValue<string>(SettingsJWTIssuer)!,
                _configuration.GetValue<string>(SettingsJWTAudience)!
            ))
        {
            return BadRequest("JWT validation fail");
        }

        var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (userEmail == null)
        {
            return BadRequest(new Message("No email in jwt"));
        }

        // get user and tokens
        var appUser = await _userManager.FindByEmailAsync(userEmail);
        if (appUser == null)
        {
            return NotFound($"User with email {userEmail} not found");
        }

        // load and compare refresh tokens

        var matchingTokens = await _context.RefreshTokens
            .Where(x =>
                x.UserId == appUser.Id &&
                ((x.RefreshToken == oldRefreshToken && x.Expiration > DateTime.UtcNow) ||
                 (x.PreviousRefreshToken == oldRefreshToken && x.PreviousExpiration > DateTime.UtcNow))
            )
            .ToListAsync();


        if (matchingTokens == null)
        {
            return Problem("RefreshTokens collection is null");
        }

        if (matchingTokens.Count == 0)
        {
            return Problem("RefreshTokens collection is empty, no valid refresh tokens found");
        }

        if (matchingTokens.Count != 1)
        {
            return Problem("More than one valid refresh token found.");
        }

        // generate new jwt

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);

        // generate jwt
        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration.GetValue<string>(SettingsJWTKey)!,
            _configuration.GetValue<string>(SettingsJWTIssuer)!,
            _configuration.GetValue<string>(SettingsJWTAudience)!,
            GetExpirationDateTime(jwtExpiresInSeconds, SettingsJWTExpiresInSeconds)
        );

        // make new refresh token, obsolete old ones
        var refreshToken = matchingTokens.First();
        if (refreshToken.RefreshToken == oldRefreshToken)
        {
            _context.Attach(refreshToken);
            refreshToken.PreviousRefreshToken = refreshToken.RefreshToken;
            refreshToken.PreviousExpiration = DateTime.UtcNow.AddMinutes(1);

            refreshToken.RefreshToken = Guid.NewGuid().ToString();
            refreshToken.Expiration =
                GetExpirationDateTime(refreshTokenExpiresInSeconds, SettingsJWTRefreshTokenExpiresInSeconds);

            await _context.SaveChangesAsync();
        }

        // var res = new JWTResponse()
        // {
        //     JWT = jwt,
        //     RefreshToken = refreshToken.RefreshToken,
        // };

        // return Ok(res);
        
        SetAuthCookies(jwt, refreshToken.RefreshToken);
        
        return Ok(new Message(["Token renewed"]));
    }

    /// <summary>
    /// Logout
    /// </summary>
    /// <returns></returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Message), StatusCodes.Status404NotFound)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    public async Task<ActionResult> Logout(
        // [FromBody] LogoutInfo logout
        )
    {
        // delete the refresh token - so user is kicked out after jwt expiration
        // We do not invalidate the jwt on serverside - that would require pipeline modification and checking against db on every request
        // so client can actually continue to use the jwt until it expires

        var refreshToken = Request.Cookies["refreshToken"];
        if (refreshToken == null)
        {
            return BadRequest(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "No refresh token provided"
                }
            );
        }
        
        var userIdStr = _userManager.GetUserId(User);
        if (userIdStr == null)
        {
            return BadRequest(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Invalid refresh token"
                }
            );
        }

        if (!Guid.TryParse(userIdStr, out var userId))
        {
            return BadRequest("Deserialization error");
        }

        var appUser = await _context.Users
            .Where(u => u.Id == userId)
            .SingleOrDefaultAsync();
        if (appUser == null)
        {
            return NotFound(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "User/Password problem"
                }
            );
        }

        var matchingTokens = await _context.RefreshTokens
            .Where(x =>
                x.UserId == appUser.Id &&
                (x.RefreshToken == refreshToken ||
                 x.PreviousRefreshToken == refreshToken)
                )
            .ToListAsync();

        foreach (var appRefreshToken in matchingTokens)
        {
            _context.RefreshTokens.Remove(appRefreshToken);
        }

        var deleteCount = await _context.SaveChangesAsync();

        ClearAuthCookies();
        
        return Ok(new { TokenDeleteCount = deleteCount });
    }
    
    /// <summary>
    /// Get current user info
    /// </summary>
    /// <returns>Current user info</returns>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(DTO.v1.Identity.CurrentUserInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<ActionResult<DTO.v1.Identity.CurrentUserInfo>> CurrentUserInfo()
    {
        var userIdStr = _userManager.GetUserId(User);
        if (userIdStr == null)
        {
            return BadRequest(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Invalid user"
                }
            );
        }
        
        Console.WriteLine(userIdStr);

        if (!Guid.TryParse(userIdStr, out var userId))
        {
            return BadRequest("Deserialization error");
        }

        var appUser = await _context.Users
            .Where(u => u.Id == userId)
            .SingleOrDefaultAsync();
        if (appUser == null)
        {
            return NotFound(
                new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "User not found"
                }
            );
        }

        var userInfo = new DTO.v1.Identity.CurrentUserInfo()
        {
            Id = appUser.Id,
            Email = appUser.Email!,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            Role = (await _userManager.GetRolesAsync(appUser)).FirstOrDefault() ?? "user",
            UniId = appUser.UniId,
            MatriculationNumber = appUser.MatriculationNumber,
            Program = appUser.Program,
        };

        return Ok(userInfo);
    }
    
    /// <summary>
    /// Update account info
    /// </summary>
    /// <param name="model">Info to update</param>
    /// <returns>Current user info</returns>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(DTO.v1.Identity.CurrentUserInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
    [HttpPut]
    public async Task<ActionResult<DTO.v1.Identity.CurrentUserInfo>> UpdateCurrentUserInfo(
        [FromBody] DTO.v1.Identity.UpdateAccountInfo model)
    {
        var userIdStr = _userManager.GetUserId(User);
        if (userIdStr == null || !Guid.TryParse(userIdStr, out var userId))
        {
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = "Invalid user"
            });
        }

        var appUser = await _userManager.FindByIdAsync(userIdStr);
        if (appUser == null)
        {
            return NotFound(new RestApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Error = "User not found"
            });
        }

        // if email is to update, check for availability
        if (!string.IsNullOrWhiteSpace(model.Email) &&
            !string.Equals(appUser.Email, model.Email, StringComparison.OrdinalIgnoreCase))
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null && existingUser.Id != appUser.Id)
            {
                return BadRequest(new RestApiErrorResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = $"Email {model.Email} is already in use"
                });
            }

            appUser.Email = model.Email;
            appUser.UserName = model.Email;
        }

        if (!string.IsNullOrWhiteSpace(model.FirstName) &&
            !string.Equals(appUser.FirstName, model.FirstName, StringComparison.Ordinal))
        {
            appUser.FirstName = model.FirstName;
        }
        if (!string.IsNullOrWhiteSpace(model.LastName) &&
            !string.Equals(appUser.LastName, model.LastName, StringComparison.Ordinal))
        {
            appUser.LastName = model.LastName;
        }
        appUser.PhoneNumber = model.PhoneNumber;
        var isTeacher = await _userManager.IsInRoleAsync(appUser, "teacher");
        var isStudent = await _userManager.IsInRoleAsync(appUser, "student");
        if (isTeacher || isStudent)
        {
            if (!string.IsNullOrWhiteSpace(model.UniId) &&
                !string.Equals(appUser.UniId, model.UniId, StringComparison.Ordinal))
            {
                appUser.UniId = model.UniId;
            }
        }

        if (isStudent)
        {
            if (!string.IsNullOrWhiteSpace(model.MatriculationNumber) &&
                !string.Equals(appUser.MatriculationNumber, model.MatriculationNumber, StringComparison.Ordinal))
            {
                appUser.MatriculationNumber = model.MatriculationNumber;
            }

            if (!string.IsNullOrWhiteSpace(model.Program) &&
                !string.Equals(appUser.Program, model.Program, StringComparison.Ordinal))
            {
                appUser.Program = model.Program;
            }
        }

        var result = await _userManager.UpdateAsync(appUser);
        if (!result.Succeeded)
        {
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = result.Errors.First().Description
            });
        }

        var currentUserInfo = new DTO.v1.Identity.CurrentUserInfo
        {
            Id = appUser.Id,
            Email = appUser.Email!,
            PhoneNumber = appUser.PhoneNumber,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            Role = (await _userManager.GetRolesAsync(appUser)).FirstOrDefault() ?? "user",
            UniId = appUser.UniId,
            MatriculationNumber = appUser.MatriculationNumber,
            Program = appUser.Program
        };

        return Ok(currentUserInfo);
    }
    
    /// <summary>
    /// Delete account (anonymize data and disable login)
    /// </summary>
    /// <returns></returns>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Message), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
    [HttpDelete]
    public async Task<ActionResult<Message>> Delete()
    {
        var userIdStr = _userManager.GetUserId(User);
        if (userIdStr == null || !Guid.TryParse(userIdStr, out var userId))
        {
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = "Invalid user"
            });
        }

        var appUser = await _userManager.FindByIdAsync(userIdStr);
        if (appUser == null)
        {
            return NotFound(new RestApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Error = "User not found"
            });
        }

        // Anonymize user data
        var deletedId = Guid.NewGuid();
        appUser.FirstName = "[Deleted]";
        appUser.LastName = "[Deleted]";
        appUser.PhoneNumber = null;
        appUser.Email = $"deleted-{deletedId}@deleted.local"; // Unique, not real email
        appUser.UserName = $"deleted-{deletedId}";
        appUser.UniId = null;
        appUser.MatriculationNumber = null;
        appUser.Program = null;
        appUser.AzureObjectId = null;

        // Disable login
        appUser.LockoutEnabled = true;
        appUser.LockoutEnd = DateTimeOffset.MaxValue;

        // Remove refresh tokens
        var refreshTokens = await _context.RefreshTokens
            .Where(t => t.UserId == appUser.Id)
            .ToListAsync();
        _context.RefreshTokens.RemoveRange(refreshTokens);

        // Remove roles
        var roles = await _userManager.GetRolesAsync(appUser);
        foreach (var role in roles)
        {
            await _userManager.RemoveFromRoleAsync(appUser, role);
        }

        // Remove claims
        var claims = await _userManager.GetClaimsAsync(appUser);
        foreach (var claim in claims)
        {
            await _userManager.RemoveClaimAsync(appUser, claim);
        }

        var result = await _userManager.UpdateAsync(appUser);
        if (!result.Succeeded)
        {
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = result.Errors.First().Description
            });
        }

        await _context.SaveChangesAsync();
        ClearAuthCookies();

        return Ok(new Message(["Account anonymized, login disabled"]));
    }

    private DateTime GetExpirationDateTime(int? expiresInSeconds, string settingsKey)
    {
        if (expiresInSeconds <= 0) expiresInSeconds = int.MaxValue;
        expiresInSeconds = expiresInSeconds < _configuration.GetValue<int>(settingsKey)
            ? expiresInSeconds
            : _configuration.GetValue<int>(settingsKey);

        return DateTime.UtcNow.AddSeconds(expiresInSeconds ?? 60);
    }
    
    private void SetAuthCookies(string jwt, string refreshToken)
    {
        Response.Cookies.Append("accessToken", jwt, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddMinutes(15)
        });

        Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/api/v1.0/Account/RenewRefreshToken",
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });
        
        Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/api/v1.0/Account/Logout",
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });
    }
    
    private void ClearAuthCookies()
    {
        Response.Cookies.Delete("accessToken", new CookieOptions
        {
            Path = "/"
        });

        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            Path = "/api/v1.0/Account/RenewRefreshToken"
        });
        
        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            Path = "/api/v1.0/Account/Logout"
        });
    }
}