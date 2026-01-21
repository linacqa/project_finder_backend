using Asp.Versioning;
using Base.Helpers;
using BLL.Contracts;
using Microsoft.AspNetCore.Mvc;
using DTO.v1;

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
        /// Get all projects
        /// </summary>
        /// <returns>List of projects</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.Project>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO.v1.Project>>> GetProjects()
        {
            var data = (await _bll.ProjectService.AllAsync()).ToList();
            
            return data.Select(d => _mapper.Map(d)!).ToList();
        }

        /// <summary>
        /// Get a single project by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Project</returns>
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
        /// Update the project by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(Guid id, DTO.v1.Project project)
        {
            if (id != project.Id)
            {
                return BadRequest();
            }

            try
            {
                await _bll.ProjectService.UpdateAsync(_mapper.Map(project)!, User.GetUserId());
                await _bll.SaveChangesAsync();
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Add a new project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(DTO.v1.Project), StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<ActionResult<DTO.v1.Project>> PostProject(ProjectCreate project)
        {
            var bllProject = _mapper.Map(project);

            try
            {
                _bll.ProjectService.Add(bllProject, User.GetUserId());
                await _bll.SaveChangesAsync();
            } catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            
            return CreatedAtAction("GetProject", new
            {
                id = bllProject.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, _mapper.Map(bllProject)!);
        }

        /// <summary>
        /// Delete the project by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
                _bll.ProjectService.Remove(project, User.GetUserId());
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
