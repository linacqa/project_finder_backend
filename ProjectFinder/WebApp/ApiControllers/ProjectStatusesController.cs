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
    public class ProjectStatussController : ControllerBase
    {
        private readonly IAppBLL _bll;
        
        private readonly DTO.v1.Mappers.ProjectStatusMapper _mapper = new();

        public ProjectStatussController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all projectStatuses
        /// </summary>
        /// <returns>List of projectStatuses</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.ProjectStatus>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO.v1.ProjectStatus>>> GetProjectStatuses()
        {
            var data = (await _bll.ProjectStatusService.AllAsync()).ToList();
            
            return data.Select(d => _mapper.Map(d)!).ToList();
        }

        /// <summary>
        /// Get a single projectStatus by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ProjectStatus</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(DTO.v1.ProjectStatus), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<DTO.v1.ProjectStatus>> GetProjectStatus(Guid id)
        {
            var projectStatus = await _bll.ProjectStatusService.FindAsync(id);

            if (projectStatus == null)
            {
                return NotFound();
            }

            return _mapper.Map(projectStatus)!;
        }

        // /// <summary>
        // /// Update the projectStatus by id (admin)
        // /// </summary>
        // /// <param name="id"></param>
        // /// <param name="projectStatus"></param>
        // /// <returns></returns>
        // [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        // [Produces("application/json")]
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutProjectStatus(Guid id, DTO.v1.ProjectStatus projectStatus)
        // {
        //     if (id != projectStatus.Id)
        //     {
        //         return BadRequest();
        //     }
        //
        //     try
        //     {
        //         await _bll.ProjectStatusService.UpdateAsync(_mapper.Map(projectStatus)!, User.GetUserId());
        //         await _bll.SaveChangesAsync();
        //     }
        //     catch (UnauthorizedAccessException e)
        //     {
        //         return Unauthorized(e.Message);
        //     }
        //
        //     return NoContent();
        // }

        // /// <summary>
        // /// Add a new projectStatus (admin)
        // /// </summary>
        // /// <param name="projectStatus"></param>
        // /// <returns></returns>
        // [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status201Created)]
        // [ProducesResponseType(typeof(DTO.v1.ProjectStatus), StatusCodes.Status200OK)]
        // [HttpPost]
        // public async Task<ActionResult<DTO.v1.ProjectStatus>> PostProjectStatus(ProjectStatusCreate projectStatus)
        // {
        //     var bllProjectStatus = _mapper.Map(projectStatus);
        //
        //     try
        //     {
        //         _bll.ProjectStatusService.Add(bllProjectStatus, User.GetUserId());
        //         await _bll.SaveChangesAsync();
        //     } catch (UnauthorizedAccessException e)
        //     {
        //         return Unauthorized(e.Message);
        //     }
        //     
        //     return CreatedAtAction("GetProjectStatus", new
        //     {
        //         id = bllProjectStatus.Id,
        //         version = HttpContext.GetRequestedApiVersion()!.ToString()
        //     }, _mapper.Map(bllProjectStatus)!);
        // }
        
        // /// <summary>
        // /// Delete the projectStatus by id (admin)
        // /// </summary>
        // /// <param name="id"></param>
        // /// <returns></returns>
        // [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        // [Produces("application/json")]
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteProjectStatus(Guid id)
        // {
        //     var projectStatus = await _bll.ProjectStatusService.FindAsync(id);
        //     if (projectStatus == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     try
        //     {
        //         _bll.ProjectStatusService.Remove(projectStatus, User.GetUserId());
        //         await _bll.SaveChangesAsync();
        //     }
        //     catch (UnauthorizedAccessException e)
        //     {
        //         return Unauthorized(e.Message);
        //     }
        //
        //     return NoContent();
        // }
    }
}
