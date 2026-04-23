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
    public class ProjectStatusesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        
        private readonly DTO.v1.Mappers.ProjectStatusMapper _mapper = new();

        public ProjectStatusesController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all projectStatuses
        /// </summary>
        /// <returns>List of projectStatuses</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    }
}
