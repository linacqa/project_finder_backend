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
    public class StepStatusesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        
        private readonly DTO.v1.Mappers.StepStatusMapper _mapper = new();

        public StepStatusesController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all stepStatuses
        /// </summary>
        /// <returns>List of stepStatuses</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.StepStatus>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO.v1.StepStatus>>> GetStepStatuses()
        {
            var data = (await _bll.StepStatusService.AllAsync()).ToList();
            
            return data.Select(d => _mapper.Map(d)!).ToList();
        }

        /// <summary>
        /// Get a single stepStatus by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>StepStatus</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(DTO.v1.StepStatus), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<DTO.v1.StepStatus>> GetStepStatus(Guid id)
        {
            var stepStatus = await _bll.StepStatusService.FindAsync(id);

            if (stepStatus == null)
            {
                return NotFound();
            }

            return _mapper.Map(stepStatus)!;
        }

        // /// <summary>
        // /// Update the stepStatus by id (admin)
        // /// </summary>
        // /// <param name="id"></param>
        // /// <param name="stepStatus"></param>
        // /// <returns></returns>
        // [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        // [Produces("application/json")]
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutStepStatus(Guid id, DTO.v1.StepStatus stepStatus)
        // {
        //     if (id != stepStatus.Id)
        //     {
        //         return BadRequest();
        //     }
        //
        //     try
        //     {
        //         await _bll.StepStatusService.UpdateAsync(_mapper.Map(stepStatus)!, User.GetUserId());
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
        // /// Add a new stepStatus (admin)
        // /// </summary>
        // /// <param name="stepStatus"></param>
        // /// <returns></returns>
        // [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status201Created)]
        // [ProducesResponseType(typeof(DTO.v1.StepStatus), StatusCodes.Status200OK)]
        // [HttpPost]
        // public async Task<ActionResult<DTO.v1.StepStatus>> PostStepStatus(StepStatusCreate stepStatus)
        // {
        //     var bllStepStatus = _mapper.Map(stepStatus);
        //
        //     try
        //     {
        //         _bll.StepStatusService.Add(bllStepStatus, User.GetUserId());
        //         await _bll.SaveChangesAsync();
        //     } catch (UnauthorizedAccessException e)
        //     {
        //         return Unauthorized(e.Message);
        //     }
        //     
        //     return CreatedAtAction("GetStepStatus", new
        //     {
        //         id = bllStepStatus.Id,
        //         version = HttpContext.GetRequestedApiVersion()!.ToString()
        //     }, _mapper.Map(bllStepStatus)!);
        // }
        
        // /// <summary>
        // /// Delete the stepStatus by id (admin)
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
        // public async Task<IActionResult> DeleteStepStatus(Guid id)
        // {
        //     var stepStatus = await _bll.StepStatusService.FindAsync(id);
        //     if (stepStatus == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     try
        //     {
        //         _bll.StepStatusService.Remove(stepStatus, User.GetUserId());
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
