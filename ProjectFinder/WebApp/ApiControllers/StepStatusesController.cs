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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    }
}
