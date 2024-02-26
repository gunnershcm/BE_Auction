using API.DTOs.Requests.Posts;
using API.DTOs.Responses.Posts;
using API.Services.Interfaces;
using Domain.Constants.Enums;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories.Interfaces;
using API.DTOs.Responses.Auctions;
using Persistence.Helpers;
using API.DTOs.Requests.Auctions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace API.Controllers
{

    [Route("/v1/auction/auction")]
    public class AuctionController : BaseController
    {
        private readonly IAuctionService _auctionService;
        private readonly IRepositoryBase<Auction> _auctionRepository;

        public AuctionController(IAuctionService auctionService, IRepositoryBase<Auction> auctionRepository)
        {
            _auctionService = auctionService;
            _auctionRepository = auctionRepository;
        }

        [Authorize]
        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<GetAuctionResponse>), 200)]
        public async Task<IActionResult> GetAllAuction()
        {
            var result = await _auctionService.Get();
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetAuctionResponse>), 200)]
        public async Task<IActionResult> GetAuctions(
            [FromQuery] string? filter,
            [FromQuery] string? sort,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            try
            {
                var result = await _auctionService.Get();
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
        [ProducesResponseType(typeof(IEnumerable<GetAuctionResponse>), 200)]
        public async Task<IActionResult> GetPostById(int id)
        {
            try
            {
                var result = await _auctionService.GetById(id);
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

        [Authorize(Roles = $"{Roles.STAFF},{Roles.ADMIN}")]
        [HttpPost("staff/new")]
        [ProducesResponseType(typeof(IEnumerable<GetAuctionResponse>), 200)]
        public async Task<IActionResult> CreateAuctionByStaff(int propertyId, [FromBody] CreateAuctionRequest model)
        {
            try
            {
                Auction entity = await _auctionService.CreateAuctionByStaff(propertyId, model);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{Roles.STAFF},{Roles.ADMIN}")]
        [HttpPut("staff/{auctionId}")]
        [ProducesResponseType(typeof(IEnumerable<GetAuctionResponse>), 200)]
        public async Task<IActionResult> UpdateAuctionByStaff(int auctionId, [FromBody] UpdateAuctionRequest model)
        {
            try
            {
                var result = await _auctionService.UpdateByStaff(auctionId , model);
                return Ok("Update Auction Successfully");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Auction is not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{Roles.STAFF},{Roles.ADMIN}")]
        [HttpDelete("staff/{auctionId}")]
        [ProducesResponseType(typeof(IEnumerable<GetAuctionResponse>), 200)]
        public async Task<IActionResult> DeleteAuction(int auctionId)
        {
            try
            {
                var auction = await _auctionRepository.FirstOrDefaultAsync(x => x.Id == auctionId);
                await _auctionService.Remove(auctionId);
                return Ok("Remove Auction Successfully");
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

