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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    }
}
