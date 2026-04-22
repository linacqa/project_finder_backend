using Asp.Versioning;
using Base.Helpers;
using BLL.Contracts;
using Microsoft.AspNetCore.Mvc;
using DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        
        private readonly DTO.v1.Mappers.ApplicationMapper _mapper = new();

        public ApplicationsController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all applications (admin)
        /// </summary>
        /// <returns>List of applications</returns>
        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.Application>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO.v1.Application>>> GetApplications()
        {
            var data = (await _bll.ApplicationService.AllAsync()).ToList();
            
            return data.Select(d => _mapper.Map(d)!).ToList();
        }

        /// <summary>
        /// Get all current user's applications
        /// </summary>
        /// <returns>List of all current user's applications</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.Application>), StatusCodes.Status200OK)]
        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<DTO.v1.Application>>> GetCurrentUsersApplications()
        {
            var data = (await _bll.ApplicationService.AllAsync(User.GetUserId())).ToList();
            
            return data.Select(d => _mapper.Map(d)!).ToList();
        }
        
        /// <summary>
        /// Get current user's application by project id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Application</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(DTO.v1.Application), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("my/{projectId}")]
        public async Task<ActionResult<DTO.v1.Application>> GetCurrentUsersApplicationByProjectId(Guid projectId)
        {
            var application = await _bll.ApplicationService.FindAsyncByProjectId(projectId, User.GetUserId());
        
            if (application == null)
            {
                return NotFound();
            }
        
            return _mapper.Map(application)!;
        }
        
        /// <summary>
        /// Get a single application by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Application</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(DTO.v1.Application), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<DTO.v1.Application>> GetApplication(Guid id)
        {
            var application = await _bll.ApplicationService.FindAsync(id);
        
            if (application == null)
            {
                return NotFound();
            }
        
            return _mapper.Map(application)!;
        }

        /// <summary>
        /// Add a new application
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [Authorize(Roles = "student", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(DTO.v1.Application), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(typeof(DTO.v1.Application), StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<ActionResult<DTO.v1.Application>> PostApplication(ApplicationCreate application)
        {
            var bllApplication = _mapper.Map(application);

            try
            {
                await _bll.ApplicationService.AddWithValidationAsync(bllApplication, User.GetUserId());
                await _bll.SaveChangesAsync();
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            
            return CreatedAtAction("GetApplication", new
            {
                id = bllApplication.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, _mapper.Map(bllApplication)!);
        }
        
        /// <summary>
        /// Accept the application (admin)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}/accept")]
        public async Task<IActionResult> AcceptApplication(Guid id)
        {
            var application = await _bll.ApplicationService.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            application.AcceptedAt = DateTime.UtcNow;
            application.DeclinedAt = null;
            await _bll.ApplicationService.UpdateAsync(application, User.GetUserId());
            await _bll.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Decline the application (admin)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}/decline")]
        public async Task<IActionResult> DeclineApplication(Guid id)
        {
            var application = await _bll.ApplicationService.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            application.DeclinedAt = DateTime.UtcNow;
            application.AcceptedAt = null;
            await _bll.ApplicationService.UpdateAsync(application, User.GetUserId());
            await _bll.SaveChangesAsync();
            return NoContent();
        }
        
        /// <summary>
        /// Delete the application by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication(Guid id)
        {
            var application = await _bll.ApplicationService.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            try
            {
                _bll.ApplicationService.Remove(application, User.GetUserId());
                await _bll.SaveChangesAsync();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
        
            return NoContent();
        }
    }
}
