using API.DTOs.Responses.Dashboards;

namespace API.Services.Implements
{
    public interface IDashboardService
    {
        Task<AuctionDashboardResponse> GetNumberOfAuctionState();
        Task<List<UserAuctionCountResponse>> GetUserForAuctionDashBoardByMonth(DateTime currentDate);
        Task<List<TransactionDashboardResponse>> GetTransactionDashBoardInCurrentYear(DateTime currentDate);
        Task<List<HistoryBiddingDashboardResponse>> GetBiddingInformationDashboard(DateTime currentDate);
    }
}
