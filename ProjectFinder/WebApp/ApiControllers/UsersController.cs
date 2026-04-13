using Asp.Versioning;
using BLL.Contracts;
using DAL.EF;
using DTO.v1.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public UsersController(AppDbContext context)
        {
            _context = context;
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
                .ToListAsync();
            
            return Ok(appUsers.Select(u => new UserInfo()
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                // UniId = u.UniId
            }));
        }

        /// <summary>
        /// Get all supervisors (admin)
        /// </summary>
        /// <returns>List of supervisors</returns>
        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    }
}