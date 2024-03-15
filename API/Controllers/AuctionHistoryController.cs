using API.DTOs.Requests.UserAuctions;
using API.DTOs.Responses.AuctionHistories;
using API.DTOs.Responses.UserAuctions;
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
    [Route("/v1/auction/history")]
    public class AuctionHistoryController : BaseController
    {
        private readonly IAuctionHistoryService _auctionHistoryService;

        public AuctionHistoryController(IAuctionHistoryService auctionHistoryService)
        {
            _auctionHistoryService = auctionHistoryService;
        }

        [Authorize]
        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<GetAuctionHistoryResponse>), 200)]
        public async Task<IActionResult> GetAllHistory()
        {
            var result = await _auctionHistoryService.Get();
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetAuctionHistoryResponse>), 200)]
        public async Task<IActionResult> GetAuctionHistories(
            [FromQuery] string? filter,
            [FromQuery] string? sort,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            try
            {
                var result = await _auctionHistoryService.Get();
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
        [ProducesResponseType(typeof(IEnumerable<GetAuctionHistoryResponse>), 200)]
        public async Task<IActionResult> GetAcutionHistoryById(int id)
        {
            try
            {
                var result = await _auctionHistoryService.GetById(id);
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
        [ProducesResponseType(typeof(IEnumerable<GetHistoryByUserResponse>), 200)]
        public async Task<IActionResult> GetHistoryByUserAvailable()
        {
            try
            {
                var result = await _auctionHistoryService.GetHistoryByUser(CurrentUserID);
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
        [HttpGet("get-user")]
        [ProducesResponseType(typeof(IEnumerable<GetHistoryByAuctionResponse>), 200)]
        public async Task<IActionResult> GetHistoryByAuction(int auctionId)
        {
            try
            {
                var result = await _auctionHistoryService.GetHistoryByAuction(auctionId);
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

    }
}
