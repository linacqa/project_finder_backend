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
    public class TagsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        
        private readonly DTO.v1.Mappers.TagMapper _mapper = new();

        public TagsController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all tags
        /// </summary>
        /// <returns>List of tags</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.Tag>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO.v1.Tag>>> GetTags()
        {
            var data = (await _bll.TagService.AllAsync()).ToList();
            
            return data.Select(d => _mapper.Map(d)!).ToList();
        }

        /// <summary>
        /// Get a single tag by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Tag</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DTO.v1.Tag), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<DTO.v1.Tag>> GetTag(Guid id)
        {
            var tag = await _bll.TagService.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            return _mapper.Map(tag)!;
        }

        /// <summary>
        /// Update the tag by id (admin)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag(Guid id, DTO.v1.Tag tag)
        {
            if (id != tag.Id)
            {
                return BadRequest();
            }
        
            try
            {
                await _bll.TagService.UpdateAsync(_mapper.Map(tag)!);
                await _bll.SaveChangesAsync();
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
        
            return NoContent();
        }

        /// <summary>
        /// Add a new tag (admin)
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(DTO.v1.Tag), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult<DTO.v1.Tag>> PostTag(TagCreate tag)
        {
            var bllTag = _mapper.Map(tag);
        
            try
            {
                _bll.TagService.Add(bllTag);
                await _bll.SaveChangesAsync();
            } catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            
            return CreatedAtAction("GetTag", new
            {
                id = bllTag.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, _mapper.Map(bllTag)!);
        }
        
        /// <summary>
        /// Delete the tag by id (admin)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(Guid id)
        {
            var tag = await _bll.TagService.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
        
            try
            {
                _bll.TagService.Remove(tag);
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
