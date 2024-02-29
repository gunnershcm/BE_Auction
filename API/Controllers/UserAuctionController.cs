using API.Services.Implements;
using Domain.Constants;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    }
}
