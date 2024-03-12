using API.DTOs.Requests.Users;
using API.Services.Implements;
using API.Services.Interfaces;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Helpers;

namespace API.Controllers;

[Route("/v1/auction/user")]
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<User>), 200)]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var result = await _userService.Get();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = $"{Roles.STAFF},{Roles.ADMIN}")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<User>), 200)]
    public async Task<IActionResult> GetUsers(
         [FromQuery] string? filter,
         [FromQuery] string? sort,
         [FromQuery] int page = 1,
         [FromQuery] int pageSize = 5)
    {
        try
        {
            var result = await _userService.Get();
            var pagedResponse = result.AsQueryable().GetPagedData(page, pageSize, filter, sort);
            return Ok(pagedResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IEnumerable<User>), 200)]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var result = await _userService.GetById(id);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("available")]
    [ProducesResponseType(typeof(IEnumerable<User>), 200)]
    public async Task<IActionResult> GetUserAvailable() 
    {
        try
        {
            var result = await _userService.GetById(CurrentUserID);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = Roles.ADMIN)]
    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<User>), 200)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest model)
    {
        try
        {
            await _userService.Create(model);
            return Ok("Created Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = Roles.ADMIN)]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(IEnumerable<User>), 200)]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest model)
    {
        try
        {
            var user = await _userService.Update(id, model);
            return Ok("Updated Successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPut("available")]
    [ProducesResponseType(typeof(IEnumerable<User>), 200)]
    public async Task<IActionResult> UpdateUserAvailable([FromBody] UpdateUserRequest model)
    {
        try
        {
            var user = await _userService.Update(CurrentUserID, model);
            return Ok("Updated Successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = Roles.ADMIN)]
    [HttpDelete("delete/{id}")]
    [ProducesResponseType(typeof(IEnumerable<User>), 200)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            await _userService.Remove(id);
            return Ok("Deleted Successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = Roles.ADMIN)]
    [HttpPut("active/{id}")]
    [ProducesResponseType(typeof(IEnumerable<User>), 200)]
    public async Task<IActionResult> ActiveUser(int id)
    {
        try
        {
            await _userService.UndoSoftDelete(id);
            return Ok("Active User Successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



    [Authorize]
    [HttpPatch("ChangeAvatar")]
    public async Task<IActionResult> UploadImageFirebase(IFormFile file)
    {
        try
        {
            var result = await _userService.UploadImageFirebase(CurrentUserID, file);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}