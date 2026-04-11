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
    public class ProjectStepsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        
        private readonly DTO.v1.Mappers.ProjectStepMapper _mapper = new();

        public ProjectStepsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // /// <summary>
        // /// Get all projectSteps
        // /// </summary>
        // /// <returns>List of projectSteps</returns>
        // [Produces("application/json")]
        // [ProducesResponseType(typeof(IEnumerable<DTO.v1.ProjectStep>), StatusCodes.Status200OK)]
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<DTO.v1.ProjectStep>>> GetProjectSteps()
        // {
        //     var data = (await _bll.ProjectStepService.AllAsync()).ToList();
        //     
        //     return data.Select(d => _mapper.Map(d)!).ToList();
        // }
        
        /// <summary>
        /// Get all projectSteps by projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>List of projectSteps</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.ProjectStep>), StatusCodes.Status200OK)]
        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IEnumerable<DTO.v1.ProjectStep>>> GetProjectStepsByProjectId(Guid projectId)
        {
            var data = (await _bll.ProjectStepService.AllAsyncByProjectId(projectId, User.GetUserId())).ToList();
            
            return data.Select(d => _mapper.Map(d)!).ToList();
        }
        
        /// <summary>
        /// Get a single projectStep by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ProjectStep</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(DTO.v1.ProjectStep), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<DTO.v1.ProjectStep>> GetProjectStep(Guid id)
        {
            var projectStep = await _bll.ProjectStepService.FindAsync(id);

            if (projectStep == null)
            {
                return NotFound();
            }

            return _mapper.Map(projectStep)!;
        }
        
        /// <summary>
        /// Update the projectStep status by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectStep"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateProjectStepStatus(Guid id, ProjectStepCreateUpdate projectStep)
        {
            var projectStepFound = await _bll.ProjectStepService.FindAsync(id);
            if (projectStepFound == null)
            {
                return NotFound();
            }

            try
            {
                projectStepFound.StepStatusId = projectStep.StepStatusId;
                await _bll.ProjectStepService.UpdateAsync(projectStepFound, User.GetUserId());
                await _bll.SaveChangesAsync();
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }

            return NoContent();
        }

        // /// <summary>
        // /// Update the projectStep by id (admin)
        // /// </summary>
        // /// <param name="id"></param>
        // /// <param name="projectStep"></param>
        // /// <returns></returns>
        // [Produces("application/json")]
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutProjectStep(Guid id, ProjectStepCreateUpdate projectStep)
        // {
        //     // if (id != projectStep.Id)
        //     // {
        //     //     return BadRequest();
        //     // }
        //
        //     try
        //     {
        //         var projectStepWithId = _mapper.Map(projectStep);
        //         projectStepWithId.Id = id;
        //         await _bll.ProjectStepService.UpdateAsync(projectStepWithId, User.GetUserId());
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
        // /// Add a new projectStep
        // /// </summary>
        // /// <param name="projectStep"></param>
        // /// <returns></returns>
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status201Created)]
        // [ProducesResponseType(typeof(DTO.v1.ProjectStep), StatusCodes.Status200OK)]
        // [HttpPost]
        // public async Task<ActionResult<DTO.v1.ProjectStep>> PostProjectStep(ProjectStepCreateUpdate projectStep)
        // {
        //     var bllProjectStep = _mapper.Map(projectStep);
        //
        //     try
        //     {
        //         _bll.ProjectStepService.Add(bllProjectStep, User.GetUserId());
        //         await _bll.SaveChangesAsync();
        //     } catch (UnauthorizedAccessException e)
        //     {
        //         return Unauthorized(e.Message);
        //     }
        //     
        //     return CreatedAtAction("GetProjectStep", new
        //     {
        //         id = bllProjectStep.Id,
        //         version = HttpContext.GetRequestedApiVersion()!.ToString()
        //     }, _mapper.Map(bllProjectStep)!);
        // }
        
        // /// <summary>
        // /// Delete the projectStep by id
        // /// </summary>
        // /// <param name="id"></param>
        // /// <returns></returns>
        // [Produces("application/json")]
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteProjectStep(Guid id)
        // {
        //     var projectStep = await _bll.ProjectStepService.FindAsync(id);
        //     if (projectStep == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     try
        //     {
        //         _bll.ProjectStepService.Remove(projectStep, User.GetUserId());
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
