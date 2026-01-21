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
    public class StepsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        
        private readonly DTO.v1.Mappers.StepMapper _mapper = new();

        public StepsController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all steps
        /// </summary>
        /// <returns>List of steps</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.Step>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO.v1.Step>>> GetSteps()
        {
            var data = (await _bll.StepService.AllAsync()).ToList();
            
            return data.Select(d => _mapper.Map(d)!).ToList();
        }

        /// <summary>
        /// Get a single step by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Step</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(DTO.v1.Step), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<DTO.v1.Step>> GetStep(Guid id)
        {
            var step = await _bll.StepService.FindAsync(id);

            if (step == null)
            {
                return NotFound();
            }

            return _mapper.Map(step)!;
        }

        // /// <summary>
        // /// Update the step by id (admin)
        // /// </summary>
        // /// <param name="id"></param>
        // /// <param name="step"></param>
        // /// <returns></returns>
        // [Produces("application/json")]
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutStep(Guid id, DTO.v1.Step step)
        // {
        //     if (id != step.Id)
        //     {
        //         return BadRequest();
        //     }
        //
        //     try
        //     {
        //         await _bll.StepService.UpdateAsync(_mapper.Map(step)!, User.GetUserId());
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
        // /// Add a new step (admin)
        // /// </summary>
        // /// <param name="step"></param>
        // /// <returns></returns>
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status201Created)]
        // [ProducesResponseType(typeof(DTO.v1.Step), StatusCodes.Status200OK)]
        // [HttpPost]
        // public async Task<ActionResult<DTO.v1.Step>> PostStep(StepCreate step)
        // {
        //     var bllStep = _mapper.Map(step);
        //
        //     try
        //     {
        //         _bll.StepService.Add(bllStep, User.GetUserId());
        //         await _bll.SaveChangesAsync();
        //     } catch (UnauthorizedAccessException e)
        //     {
        //         return Unauthorized(e.Message);
        //     }
        //     
        //     return CreatedAtAction("GetStep", new
        //     {
        //         id = bllStep.Id,
        //         version = HttpContext.GetRequestedApiVersion()!.ToString()
        //     }, _mapper.Map(bllStep)!);
        // }
        //
        // /// <summary>
        // /// Delete the step by id (admin)
        // /// </summary>
        // /// <param name="id"></param>
        // /// <returns></returns>
        // [Produces("application/json")]
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteStep(Guid id)
        // {
        //     var step = await _bll.StepService.FindAsync(id);
        //     if (step == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     try
        //     {
        //         _bll.StepService.Remove(step, User.GetUserId());
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
