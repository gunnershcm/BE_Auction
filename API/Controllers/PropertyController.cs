using API.DTOs.Responses.Properties;
using API.Services.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("/v1/auction/property")]
    public class PropertyController : BaseController
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService proppertyService)
        {
            _propertyService = proppertyService;
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
    }
}
