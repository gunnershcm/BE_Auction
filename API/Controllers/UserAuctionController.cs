using API.DTOs.Requests.Posts;
using API.DTOs.Requests.UserAuctions;
using API.DTOs.Responses.Posts;
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
    [Route("/v1/auction/userauction")]
    public class UserAuctionController : BaseController
    {
        private readonly IUserAuctionService _userAuctionService;
        private readonly IRepositoryBase<UserAuction> _userAuctionRepository;

        public UserAuctionController(IUserAuctionService userAuctionService,
            IRepositoryBase<UserAuction> userAuctionRepository)
        {
            _userAuctionService = userAuctionService;
            _userAuctionRepository = userAuctionRepository;
        }

        [Authorize]
        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<GetUserAuctionResponse>), 200)]
        public async Task<IActionResult> GetAllUserAuction()
        {
            var result = await _userAuctionService.Get();
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetUserAuctionResponse>), 200)]
        public async Task<IActionResult> GetUserAuctions(
            [FromQuery] string? filter,
            [FromQuery] string? sort,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            try
            {
                var result = await _userAuctionService.Get();
                var pagedResponse = result.AsQueryable().GetPagedData(page, pageSize, filter, sort);
                return Ok(pagedResponse);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("get-by-auction/{auctionId}")]
        [ProducesResponseType(typeof(IEnumerable<GetUserByAuctionResponse>), 200)]
        public async Task<IActionResult> GetUserByAuction(int auctionId,
            [FromQuery] string? filter,
            [FromQuery] string? sort,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            try
            {
                var result = await _userAuctionService.GetUserByAuction(auctionId);
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
        [ProducesResponseType(typeof(IEnumerable<GetUserAuctionResponse>), 200)]
        public async Task<IActionResult> GetUserAuctionById(int id)
        {
            try
            {
                var result = await _userAuctionService.GetById(id);
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
        [HttpGet("get-top3")]
        [ProducesResponseType(typeof(IEnumerable<GetUserByAuctionResponse>), 200)]
        public async Task<IActionResult> GetUserTop3ByAuction(int auctionId)
        {
            try
            {
                var result = await _userAuctionService.GetUserTop3ByAuction(auctionId);
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
        [ProducesResponseType(typeof(IEnumerable<GetAuctionByUserResponse>), 200)]
        public async Task<IActionResult> GetAuctionByUserAvailable()
        {
            try
            {
                var result = await _userAuctionService.GetAuctionByUser(CurrentUserID);
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

        [Authorize(Roles = Roles.MEMBER)]
        [HttpGet("check-join-auction")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> CheckUserJoinAuction(int auctionId)
        {
            try
            {
                bool isJoined = await _userAuctionService.CheckUserJoinedAuction(CurrentUserID, auctionId);
                return Ok(isJoined);
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

        [Authorize(Roles = Roles.MEMBER)]
        [HttpPost("member/join-auction")]
        public async Task<IActionResult> JoinAuction(int auctionId)
        {
            try
            {
                await _userAuctionService.JoinAuction(CurrentUserID, auctionId);
                return Ok("Join Auction successfully");
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

        [Authorize(Roles = Roles.MEMBER)]
        [HttpPatch("bidding-amount")]
        [ProducesResponseType(typeof(IEnumerable<BiddingAmountRequest>), 200)]
        public async Task<IActionResult> BiddingAmount(int auctionId, [FromBody] BiddingAmountRequest model)
        {
            try
            {
                await _userAuctionService.BiddingAmount(CurrentUserID, auctionId, model);
                return Ok("Bidding Amount Successfully");
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
       
    }
}
