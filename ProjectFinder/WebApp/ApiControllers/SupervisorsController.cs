using Asp.Versioning;
using BLL.Contracts;
using DAL.EF;
using DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SupervisorsController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public SupervisorsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all supervisors
        /// </summary>
        /// <returns>List of supervisors</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.Identity.SupervisorInfo>), StatusCodes.Status200OK)]
        [HttpGet]
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
    }
}