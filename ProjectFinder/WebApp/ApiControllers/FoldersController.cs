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
    public class FoldersController : ControllerBase
    {
        private readonly IAppBLL _bll;
        
        private readonly DTO.v1.Mappers.FolderMapper _mapper = new();

        public FoldersController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all folders
        /// </summary>
        /// <returns>List of folders</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DTO.v1.Folder>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO.v1.Folder>>> GetFolders()
        {
            var data = (await _bll.FolderService.AllAsync()).ToList();
            
            return data.Select(d => _mapper.Map(d)!).ToList();
        }

        /// <summary>
        /// Get a single folder by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Folder</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DTO.v1.Folder), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<DTO.v1.Folder>> GetFolder(Guid id)
        {
            var folder = await _bll.FolderService.FindAsync(id);

            if (folder == null)
            {
                return NotFound();
            }

            return _mapper.Map(folder)!;
        }

        // /// <summary>
        // /// Update the folder by id (admin)
        // /// </summary>
        // /// <param name="id"></param>
        // /// <param name="folder"></param>
        // /// <returns></returns>
        // [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        // [Produces("application/json")]
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutFolder(Guid id, DTO.v1.Folder folder)
        // {
        //     if (id != folder.Id)
        //     {
        //         return BadRequest();
        //     }
        //
        //     try
        //     {
        //         await _bll.FolderService.UpdateAsync(_mapper.Map(folder)!);
        //         await _bll.SaveChangesAsync();
        //     }
        //     catch (UnauthorizedAccessException e)
        //     {
        //         return Unauthorized(e.Message);
        //     }
        //
        //     return NoContent();
        // }
        //
        // /// <summary>
        // /// Add a new folder (admin)
        // /// </summary>
        // /// <param name="folder"></param>
        // /// <returns></returns>
        // [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(typeof(DTO.v1.Folder), StatusCodes.Status201Created)]
        // [HttpPost]
        // public async Task<ActionResult<DTO.v1.Folder>> PostFolder(FolderCreate folder)
        // {
        //     var bllFolder = _mapper.Map(folder);
        //
        //     try
        //     {
        //         _bll.FolderService.Add(bllFolder);
        //         await _bll.SaveChangesAsync();
        //     } catch (UnauthorizedAccessException e)
        //     {
        //         return Unauthorized(e.Message);
        //     }
        //     
        //     return CreatedAtAction("GetFolder", new
        //     {
        //         id = bllFolder.Id,
        //         version = HttpContext.GetRequestedApiVersion()!.ToString()
        //     }, _mapper.Map(bllFolder)!);
        // }
        //
        // /// <summary>
        // /// Delete the folder by id (admin)
        // /// </summary>
        // /// <param name="id"></param>
        // /// <returns></returns>
        // [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        // [Produces("application/json")]
        // [Consumes("application/json")]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteFolder(Guid id)
        // {
        //     var folder = await _bll.FolderService.FindAsync(id);
        //     if (folder == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     try
        //     {
        //         _bll.FolderService.Remove(folder);
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
