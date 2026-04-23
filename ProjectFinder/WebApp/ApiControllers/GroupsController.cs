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
    public class GroupsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        
        private readonly DTO.v1.Mappers.GroupMapper _mapper = new();

        public GroupsController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all groups
        /// </summary>
        /// <returns>List of groups</returns>
        [Authorize(Roles = "student,admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.Group>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO.v1.Group>>> GetGroups()
        {
            var data = (await _bll.GroupService.AllAsync()).ToList();
            
            return data.Select(d => _mapper.Map(d)!).ToList();
        }
        
        /// <summary>
        /// Get current user's groups that match the project's team size
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>List of matching groups</returns>
        [Authorize(Roles = "student,admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.Group>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("matchingProjectTeamSize/{projectId}")]
        public async Task<ActionResult<IEnumerable<DTO.v1.Group>>> GetUserGroupsMatchingProjectTeamSize(Guid projectId)
        {
            var userId = User.GetUserId();
            var project = await _bll.ProjectService.FindAsync(projectId);

            if (project == null)
            {
                return NotFound();
            }
            
            var data = await _bll.GroupService.AllAsyncMatchingTeamSize(project.MinStudents, project.MaxStudents, userId);

            return data.Select(g => _mapper.Map(g)!).ToList();
        }
        
        /// <summary>
        /// Get a single group by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Group</returns>
        [Authorize(Roles = "student,admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DTO.v1.Group), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<DTO.v1.Group>> GetGroup(Guid id)
        {
            var group = await _bll.GroupService.FindAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            return _mapper.Map(group)!;
        }

        /// <summary>
        /// Add a new group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [Authorize(Roles = "student,admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(DTO.v1.Group), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult<DTO.v1.Group>> PostGroup(GroupCreateUpdate group)
        {
            var bllGroup = _mapper.Map(group);
        
            try
            {
                _bll.GroupService.Add(bllGroup, User.GetUserId());
                await _bll.SaveChangesAsync();
            } catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            
            return CreatedAtAction("GetGroup", new
            {
                id = bllGroup.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, _mapper.Map(bllGroup)!);
        }
        
        /// <summary>
        /// Delete the group by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "student,admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            var group = await _bll.GroupService.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }
        
            try
            {
                _bll.GroupService.Remove(group, User.GetUserId());
                await _bll.SaveChangesAsync();
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
        
            return NoContent();
        }
        
        /// <summary>
        /// Delete the group member by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "student,admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("member/{id}")]
        public async Task<IActionResult> DeleteGroupMember(Guid id)
        {
            var userGroup = await _bll.UserGroupService.FindAsync(id);
            if (userGroup == null)
            {
                return NotFound();
            }

            try
            {
                _bll.UserGroupService.Remove(userGroup, User.GetUserId());
                await _bll.SaveChangesAsync();
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        
            return NoContent();
        }
    }
}
