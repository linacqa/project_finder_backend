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
        
        /// <summary>
        /// Get all projectSteps by projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>List of projectSteps</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.ProjectStep>), StatusCodes.Status200OK)]
        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IEnumerable<DTO.v1.ProjectStep>>> GetProjectStepsByProjectId(Guid projectId)
        {
            try
            {
                var data = (await _bll.ProjectStepService.AllAsyncByProjectId(projectId, User.GetUserId())).ToList();
                return data.Select(d => _mapper.Map(d)!).ToList();
            } catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
        }
        
        /// <summary>
        /// Get a single projectStep by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ProjectStep</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    }
}
