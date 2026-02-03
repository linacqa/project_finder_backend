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
    public class ProjectTypesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        
        private readonly DTO.v1.Mappers.ProjectTypeMapper _mapper = new();

        public ProjectTypesController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all projectTypes
        /// </summary>
        /// <returns>List of projectTypes</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.ProjectType>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO.v1.ProjectType>>> GetProjectTypes()
        {
            var data = (await _bll.ProjectTypeService.AllAsync()).ToList();
            
            return data.Select(d => _mapper.Map(d)!).ToList();
        }

        /// <summary>
        /// Get a single projectType by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ProjectType</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(DTO.v1.ProjectType), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<DTO.v1.ProjectType>> GetProjectType(Guid id)
        {
            var projectType = await _bll.ProjectTypeService.FindAsync(id);

            if (projectType == null)
            {
                return NotFound();
            }

            return _mapper.Map(projectType)!;
        }

        // /// <summary>
        // /// Update the projectType by id (admin)
        // /// </summary>
        // /// <param name="id"></param>
        // /// <param name="projectType"></param>
        // /// <returns></returns>
        // [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        // [Produces("application/json")]
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutProjectType(Guid id, DTO.v1.ProjectType projectType)
        // {
        //     if (id != projectType.Id)
        //     {
        //         return BadRequest();
        //     }
        //
        //     try
        //     {
        //         await _bll.ProjectTypeService.UpdateAsync(_mapper.Map(projectType)!, User.GetUserId());
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
        // /// Add a new projectType (admin)
        // /// </summary>
        // /// <param name="projectType"></param>
        // /// <returns></returns>
        // [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status201Created)]
        // [ProducesResponseType(typeof(DTO.v1.ProjectType), StatusCodes.Status200OK)]
        // [HttpPost]
        // public async Task<ActionResult<DTO.v1.ProjectType>> PostProjectType(ProjectTypeCreate projectType)
        // {
        //     var bllProjectType = _mapper.Map(projectType);
        //
        //     try
        //     {
        //         _bll.ProjectTypeService.Add(bllProjectType, User.GetUserId());
        //         await _bll.SaveChangesAsync();
        //     } catch (UnauthorizedAccessException e)
        //     {
        //         return Unauthorized(e.Message);
        //     }
        //     
        //     return CreatedAtAction("GetProjectType", new
        //     {
        //         id = bllProjectType.Id,
        //         version = HttpContext.GetRequestedApiVersion()!.ToString()
        //     }, _mapper.Map(bllProjectType)!);
        // }
        
        // /// <summary>
        // /// Delete the projectType by id (admin)
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
        // public async Task<IActionResult> DeleteProjectType(Guid id)
        // {
        //     var projectType = await _bll.ProjectTypeService.FindAsync(id);
        //     if (projectType == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     try
        //     {
        //         _bll.ProjectTypeService.Remove(projectType, User.GetUserId());
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
