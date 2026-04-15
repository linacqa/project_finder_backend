using System.Net;
using Asp.Versioning;
using Base.Helpers;
using BLL.Contracts;
using DAL.EF;
using Domain.Identity;
using DTO.v1.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Services.Email;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        
        public UsersController(AppDbContext context, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }
        
        /// <summary>
        /// Get all users (admin)
        /// </summary>
        /// <returns>List of users</returns>
        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.Identity.UserInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO.v1.Identity.UserInfo>>> GetUsers()
        {
            var appUsers = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ToListAsync();
            
            return Ok(appUsers.Select(u => new UserInfo()
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                Role = u.UserRoles.FirstOrDefault().Role.Name
                // UniId = u.UniId
            }));
        }

        /// <summary>
        /// Get all supervisors
        /// </summary>
        /// <returns>List of supervisors</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.Identity.SupervisorInfo>), StatusCodes.Status200OK)]
        [HttpGet("Supervisors")]
        public async Task<ActionResult<IEnumerable<DTO.v1.Identity.SupervisorInfo>>> GetSupervisors()
        {
            var appUsers = await _context.Users
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name.Equals("teacher")))
                .ToListAsync();
            
            return Ok(appUsers.Select(u => new SupervisorInfo()
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                UniId = u.UniId
            }));
        }

        /// <summary>
        /// Get all students
        /// </summary>
        /// <returns>List of students</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.Identity.StudentInfo>), StatusCodes.Status200OK)]
        [HttpGet("Students")]
        public async Task<ActionResult<IEnumerable<DTO.v1.Identity.StudentInfo>>> GetStudents()
        {
            var appUsers = await _context.Users
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name.Equals("student")))
                .ToListAsync();
            
            return Ok(appUsers.Select(u => new StudentInfo()
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                UniId = u.UniId,
                MatriculationNumber = u.MatriculationNumber,
                Program = u.Program
            }));
        }
        
        /// <summary>
        /// Email all admin users
        /// </summary>
        /// <param name="request">Email payload</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("EmailAdmins")]
        public async Task<IActionResult> EmailAdmins([FromBody] AdminEmailRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var admins = await _userManager.GetUsersInRoleAsync("admin");
            // var recipients = admins
            //     .Where(u => !string.IsNullOrWhiteSpace(u.Email))
            //     .Select(u => u.Email!)
            //     .Distinct(StringComparer.OrdinalIgnoreCase)
            //     .ToList();
            // TODO: change to admins emails before prod
            var recipients = new List<string>()
            {
            };

            if (recipients.Count == 0)
            {
                return NotFound(new RestApiErrorResponse
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "No admin users with an email address were found."
                });
            }

            try
            {
                var subject = string.IsNullOrWhiteSpace(request.Subject)
                    ? "ProjectFinder message"
                    : request.Subject.Trim();

                await _emailService.SendBulkAsync(recipients, subject, request.Body, cancellationToken);

                return Ok(new { sentTo = recipients.Count });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new RestApiErrorResponse
                {
                    Status = HttpStatusCode.InternalServerError,
                    Error = $"Failed to send email: {e.Message}"
                });
            }
        }

        /// <summary>
        /// Change user's role (admin)
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeUsersRole(Guid id, string role)
        {
            var currentUserId = User.GetUserId();
            if (currentUserId == id)
            {
                return BadRequest("You cannot change your own role");
            }
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
            
            if (user == null)
            {
                return NotFound();
            }

            var userRole = user.UserRoles.FirstOrDefault(ur => ur.Role.Name.Equals(role));
            if (userRole != null)
            {
                return BadRequest($"User already has the role {role}");
            }

            var newRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name.Equals(role));
            if (newRole == null)
            {
                return BadRequest($"Role {role} does not exist");
            }

            var res = await _userManager.AddToRoleAsync(user, newRole.Name);
            
            if (!res.Succeeded)
            {
                return BadRequest(
                    new RestApiErrorResponse()
                    {
                        Status = HttpStatusCode.BadRequest,
                        Error = res.Errors.First().Description
                    }
                );
            }
            
            // remove other roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToRemove = currentRoles
                .Where(r => !string.Equals(r, newRole.Name, StringComparison.OrdinalIgnoreCase))
                .ToList();
            if (rolesToRemove.Count > 0)
            {
                var removeRes = _userManager.RemoveFromRolesAsync(user, rolesToRemove).Result;
                if (!removeRes.Succeeded)
                {
                    return BadRequest(
                        new RestApiErrorResponse()
                        {
                            Status = HttpStatusCode.BadRequest,
                            Error = removeRes.Errors.First().Description
                        });
                }
            }

            return NoContent();
        }
    }
}