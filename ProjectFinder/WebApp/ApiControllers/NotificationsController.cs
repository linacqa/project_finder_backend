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
    public class NotificationsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        
        private readonly DTO.v1.Mappers.NotificationMapper _mapper = new();

        public NotificationsController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all notifications
        /// </summary>
        /// <returns>List of notifications</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.Notification>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO.v1.Notification>>> GetNotifications()
        {
            var data = (await _bll.NotificationService.AllAsync()).ToList();
            
            return data.Select(d => _mapper.Map(d)!).ToList();
        }

        /// <summary>
        /// Get a single notification by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Notification</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(DTO.v1.Notification), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<DTO.v1.Notification>> GetNotification(Guid id)
        {
            var notification = await _bll.NotificationService.FindAsync(id);

            if (notification == null)
            {
                return NotFound();
            }

            return _mapper.Map(notification)!;
        }

        /// <summary>
        /// Update the notification by id (admin)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="notification"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotification(Guid id, DTO.v1.Notification notification)
        {
            if (id != notification.Id)
            {
                return BadRequest();
            }
        
            try
            {
                await _bll.NotificationService.UpdateAsync(_mapper.Map(notification)!, User.GetUserId());
                await _bll.SaveChangesAsync();
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
        
            return NoContent();
        }

        /// <summary>
        /// Add a new notification (admin)
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(DTO.v1.Notification), StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<ActionResult<DTO.v1.Notification>> PostNotification(NotificationCreate notification)
        {
            var bllNotification = _mapper.Map(notification);
        
            try
            {
                _bll.NotificationService.Add(bllNotification, User.GetUserId());
                await _bll.SaveChangesAsync();
            } catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            
            return CreatedAtAction("GetNotification", new
            {
                id = bllNotification.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, _mapper.Map(bllNotification)!);
        }
        
        /// <summary>
        /// Delete the notification by id (admin)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(Guid id)
        {
            var notification = await _bll.NotificationService.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
        
            try
            {
                _bll.NotificationService.Remove(notification, User.GetUserId());
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
