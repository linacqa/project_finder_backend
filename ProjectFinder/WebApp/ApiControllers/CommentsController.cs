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
    public class CommentsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        
        private readonly DTO.v1.Mappers.CommentMapper _mapper = new();

        public CommentsController(IAppBLL bll)
        {
            _bll = bll;
        }
        
        /// <summary>
        /// Get comments by project id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Comment</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.Comment>), StatusCodes.Status200OK)]
        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IEnumerable<DTO.v1.Comment>>> GetCommentsByProjectId(Guid projectId)
        {
            try
            {
                var comments = await _bll.CommentService.AllAsyncByProjectId(projectId, User.GetUserId());

                return comments.Select(c => _mapper.Map(c)!).ToList();
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
        }
        
        /// <summary>
        /// Get a single comment by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Comment</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DTO.v1.Comment), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<DTO.v1.Comment>> GetComment(Guid id)
        {
            var comment = await _bll.CommentService.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return _mapper.Map(comment)!;
        }

        /// <summary>
        /// Add a new comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(DTO.v1.Comment), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult<DTO.v1.Comment>> PostComment(CommentCreateUpdate comment)
        {
            var bllComment = _mapper.Map(comment);
        
            try
            {
                await _bll.CommentService.AddAsync(bllComment, User.GetUserId());
                await _bll.SaveChangesAsync();
            } catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            
            return CreatedAtAction("GetComment", new
            {
                id = bllComment.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, _mapper.Map(bllComment)!);
        }
        
        /// <summary>
        /// Delete the comment by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var comment = await _bll.CommentService.FindAsync(id, User.GetUserId());
            if (comment == null)
            {
                return NotFound();
            }
        
            try
            {
                _bll.CommentService.Remove(comment, User.GetUserId());
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
