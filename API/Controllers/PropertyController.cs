using API.DTOs.Requests.Posts;
using API.DTOs.Requests.Properties;
using API.DTOs.Responses.Properties;
using API.Services.Implements;
using API.Services.Interfaces;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Helpers;
using Persistence.Repositories.Interfaces;

namespace API.Controllers
{
    [Route("/v1/auction/property")]
    public class PropertyController : BaseController
    {
        private readonly IPropertyService _propertyService;
        private readonly IRepositoryBase<Property> _propertyRepository;

        public PropertyController(IPropertyService proppertyService, IRepositoryBase<Property> propertyRepository)
        {
            _propertyService = proppertyService;
            _propertyRepository = propertyRepository;
        }

        [Authorize]
        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<GetPropertyResponse>), 200)]
        public async Task<IActionResult> GetAllProperties()
        {
            try
            {
                var result = await _propertyService.Get();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<GetPropertyResponse>), 200)]
        public async Task<IActionResult> GetPropertyById(int id)
        {
            try
            {
                var result = await _propertyService.GetById(id);
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
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetPropertyResponse>), 200)]
        public async Task<IActionResult> GetProperties(
           [FromQuery] string? filter,
           [FromQuery] string? sort,
           [FromQuery] int itemCount,
           [FromQuery] int page = 1,
           [FromQuery] int pageSize = 5)
        {
            try
            {
                var result = await _propertyService.Get();
                var pagedResponse = result.AsQueryable().GetPagedData(page, pageSize, filter, sort, itemCount);
                return Ok(pagedResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = Roles.STAFF)]
        //[HttpPost("new")]
        //[ProducesResponseType(typeof(IEnumerable<GetPropertyResponse>), 200)]
        //public async Task<IActionResult> CreateProperty(int postId, [FromBody] CreatePropertyRequest model)
        //{
        //    try
        //    {
        //        Property entity = await _propertyService.CreateProperty(postId,model);
        //        return Ok(entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [Authorize(Roles = Roles.STAFF)]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IEnumerable<GetPropertyResponse>), 200)]
        public async Task<IActionResult> UpdateProperty(int id, [FromBody] UpdatePropertyRequest model)
        {
            try
            {
                var result = await _propertyService.UpdateProperty(id, model);
                return Ok("Update Property Successfully");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Property is not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.STAFF)]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(IEnumerable<GetPropertyResponse>), 200)]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                var post = await _propertyRepository.FirstOrDefaultAsync(x => x.Id == id);
                await _propertyService.Remove(id);
                return Ok("Remove Property Successfully");
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
}
