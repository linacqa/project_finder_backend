using Asp.Versioning;
using Base.DTO;
using Base.Helpers;
using BLL.Contracts;
using Microsoft.AspNetCore.Mvc;
using DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        
        private readonly DTO.v1.Mappers.ProjectMapper _mapper = new();

        public ProjectsController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Search projects
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns>List of matching projects</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PageResult<DTO.v1.Project>), StatusCodes.Status200OK)]
        [HttpGet("search")]
        public async Task<ActionResult<PageResult<DTO.v1.Project>>> SearchProjects([FromQuery] ProjectsSearchRequest searchRequest)
        {
            var data = (await _bll.ProjectService.SearchAsync(searchRequest));

            return new PageResult<DTO.v1.Project>()
            {
                Page = data.Page,
                PageSize = data.PageSize,
                TotalCount = data.TotalCount,
                Data = data.Data.Select(e => _mapper.Map(e)!).ToList(),
            };
        }

        /// <summary>
        /// Get all projects
        /// </summary>
        /// <returns>List of projects</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.Project>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO.v1.Project>>> GetProjects()
        {
            var data = (await _bll.ProjectService.AllAsync()).ToList();
            
            return data.Select(d => _mapper.Map(d)!).ToList();
        }
        
        /// <summary>
        /// Get all current user's projects
        /// </summary>
        /// <returns>List of projects</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.Project>), StatusCodes.Status200OK)]
        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<DTO.v1.Project>>> GetCurrentUsersProjects()
        {
            var userId = User.GetUserId();
            try
            {
                var data = (await _bll.ProjectService.AllCurrentUserAsync(userId)).ToList();
                return data.Select(d => _mapper.Map(d)!).ToList();
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
        }

        /// <summary>
        /// Get a single project by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Project</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DTO.v1.Project), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<DTO.v1.Project>> GetProject(Guid id)
        {
            var project = await _bll.ProjectService.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return _mapper.Map(project)!;
        }

        /// <summary>
        /// Update the project by id (admin)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(Guid id, DTO.v1.ProjectCreate project)
        {
            try
            {
                var projectToUpdate = _mapper.Map(project);
                projectToUpdate.Id = id;
                await _bll.ProjectService.UpdateAsync(projectToUpdate);
                await _bll.SaveChangesAsync();
            } catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Add a new project (admin)
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(DTO.v1.Project), StatusCodes.Status201Created)]
        // [ProducesResponseType(typeof(DTO.v1.Project), StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<ActionResult<DTO.v1.Project>> PostProject(ProjectCreate project)
        {
            var bllProject = _mapper.Map(project);

            try
            {
                _bll.ProjectService.Add(bllProject);
                await _bll.SaveChangesAsync();
            } catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            } catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            
            return CreatedAtAction("GetProject", new
            {
                id = bllProject.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, _mapper.Map(bllProject)!);
        }

        /// <summary>
        /// Delete the project by id (admin)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var project = await _bll.ProjectService.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            try
            {
                _bll.ProjectService.Remove(project);
                await _bll.SaveChangesAsync();
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }

            return NoContent();
        }
    }
}
