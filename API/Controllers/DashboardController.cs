using API.DTOs.Responses.Auctions;
using API.DTOs.Responses.Dashboards;
using API.Services.Implements;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Route("/v1/auction/dashboard")]
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpGet("admin/auction/get-auction-state")]
        [ProducesResponseType(typeof(IEnumerable<AuctionDashboardResponse>), 200)]
        public async Task<IActionResult> GetAuctionDashboard()
        {
            var result = await _dashboardService.GetNumberOfAuctionState();
            return Ok(result);
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpGet("admin/auction/get-user-by-month")]
        [ProducesResponseType(typeof(IEnumerable<UserAuctionCountResponse>), 200)]
        public async Task<IActionResult> GetCreatedTicketThisMonth()
        {
            var dashboard = await _dashboardService.GetUserForAuctionDashBoardByMonth(DateTime.Today);
            return Ok(dashboard);
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpGet("admin/auction/get-transaction-in-year")]
        [ProducesResponseType(typeof(IEnumerable<TransactionDashboardResponse>), 200)]
        public async Task<IActionResult> GetTransactionThisMonth()
        {
            var dashboard = await _dashboardService.GetTransactionDashBoardInCurrentYear(DateTime.Today);
            return Ok(dashboard);
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpGet("admin/auction/get-bidding-in-month")]
        [ProducesResponseType(typeof(IEnumerable<HistoryBiddingDashboardResponse>), 200)]
        public async Task<IActionResult> GetBiddingInformation()
        {
            var dashboard = await _dashboardService.GetBiddingInformationDashboard(DateTime.Today);
            return Ok(dashboard);
        }
    }
}

