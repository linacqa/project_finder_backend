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
    public class InvitationsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        
        private readonly DTO.v1.Mappers.InvitationMapper _mapper = new();

        public InvitationsController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all current user's invitations
        /// </summary>
        /// <returns>List of current user's invitations</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.Invitation>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO.v1.Invitation>>> GetInvitations()
        {
            var userId = User.GetUserId();
            
            var data = (await _bll.InvitationService.AllAsyncToUser(userId)).ToList();
            
            return data.Select(d => _mapper.Map(d)!).ToList();
        }

        /// <summary>
        /// Get a single invitation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Invitation</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(DTO.v1.Invitation), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<DTO.v1.Invitation>> GetInvitation(Guid id)
        {
            var invitation = await _bll.InvitationService.FindAsync(id);
        
            if (invitation == null)
            {
                return NotFound();
            }
        
            return _mapper.Map(invitation)!;
        }
        
        /// <summary>
        /// Accept the invitation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}/accept")]
        public async Task<IActionResult> AcceptInvitation(Guid id)
        {
            var invitation = await _bll.InvitationService.FindAsync(id);
            if (invitation == null)
            {
                return NotFound();
            }

            invitation.AcceptedAt = DateTime.UtcNow;
            invitation.DeclinedAt = null;
            
            try
            {
                await _bll.InvitationService.UpdateAsync(invitation, User.GetUserId());
                await _bll.SaveChangesAsync();
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            return NoContent();
        }

        /// <summary>
        /// Decline the invitation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}/decline")]
        public async Task<IActionResult> DeclineInvitation(Guid id)
        {
            var invitation = await _bll.InvitationService.FindAsync(id);
            if (invitation == null)
            {
                return NotFound();
            }

            invitation.DeclinedAt = DateTime.UtcNow;
            invitation.AcceptedAt = null;
            
            try
            {
                await _bll.InvitationService.UpdateAsync(invitation, User.GetUserId());
                await _bll.SaveChangesAsync();
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            
            return NoContent();
        }
        
        // /// <summary>
        // /// Update the invitation by id (admin)
        // /// </summary>
        // /// <param name="id"></param>
        // /// <param name="invitation"></param>
        // /// <returns></returns>
        // [Produces("application/json")]
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutInvitation(Guid id, InvitationCreateUpdate invitation)
        // {
        //     // if (id != invitation.Id)
        //     // {
        //     //     return BadRequest();
        //     // }
        //
        //     try
        //     {
        //         var invitationWithId = _mapper.Map(invitation);
        //         invitationWithId.Id = id;
        //         await _bll.InvitationService.UpdateAsync(invitationWithId, User.GetUserId());
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
        /// Send a new invitation
        /// </summary>
        /// <param name="invitation"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(DTO.v1.Invitation), StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<ActionResult<DTO.v1.Invitation>> PostInvitation(InvitationCreate invitation)
        {
            var bllInvitation = _mapper.Map(invitation);
        
            try
            {
                _bll.InvitationService.Add(bllInvitation, User.GetUserId());
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
            
            return CreatedAtAction("GetInvitation", new
            {
                id = bllInvitation.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, _mapper.Map(bllInvitation)!);
        }
        
        // /// <summary>
        // /// Delete the invitation by id
        // /// </summary>
        // /// <param name="id"></param>
        // /// <returns></returns>
        // [Produces("application/json")]
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteInvitation(Guid id)
        // {
        //     var invitation = await _bll.InvitationService.FindAsync(id);
        //     if (invitation == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     try
        //     {
        //         _bll.InvitationService.Remove(invitation, User.GetUserId());
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
