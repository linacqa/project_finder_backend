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

        // /// <summary>
        // /// Get all comments
        // /// </summary>
        // /// <returns>List of comments</returns>
        // [Produces("application/json")]
        // [ProducesResponseType(typeof(IEnumerable<DTO.v1.Comment>), StatusCodes.Status200OK)]
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<DTO.v1.Comment>>> GetComments()
        // {
        //     var data = (await _bll.CommentService.AllAsync()).ToList();
        //     
        //     return data.Select(d => _mapper.Map(d)!).ToList();
        // }
        
        /// <summary>
        /// Get comments by project id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Comment</returns>
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

        // /// <summary>
        // /// Update the comment by id (admin)
        // /// </summary>
        // /// <param name="id"></param>
        // /// <param name="comment"></param>
        // /// <returns></returns>
        // [Produces("application/json")]
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutComment(Guid id, CommentCreateUpdate comment)
        // {
        //     // if (id != comment.Id)
        //     // {
        //     //     return BadRequest();
        //     // }
        //
        //     try
        //     {
        //         var commentWithId = _mapper.Map(comment);
        //         commentWithId.Id = id;
        //         await _bll.CommentService.UpdateAsync(commentWithId, User.GetUserId());
        //         await _bll.SaveChangesAsync();
        //     }
        //     catch (UnauthorizedAccessException e)
        //     {
        //         return Unauthorized(e.Message);
        //     }
        //
        //     return NoContent();
        // }

        /// <summary>
        /// Add a new comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(DTO.v1.Comment), StatusCodes.Status201Created)]
        // [ProducesResponseType(typeof(DTO.v1.Comment), StatusCodes.Status200OK)]
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
        
        // /// <summary>
        // /// Delete the comment by id
        // /// </summary>
        // /// <param name="id"></param>
        // /// <returns></returns>
        // [Produces("application/json")]
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteComment(Guid id)
        // {
        //     var comment = await _bll.CommentService.FindAsync(id);
        //     if (comment == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     try
        //     {
        //         _bll.CommentService.Remove(comment, User.GetUserId());
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
