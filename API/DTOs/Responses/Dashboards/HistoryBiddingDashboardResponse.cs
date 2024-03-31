using Domain.Models;

namespace API.DTOs.Responses.Dashboards
{
    public class HistoryBiddingDashboardResponse
    {
        public int AuctionId { get; set; }
        public string AuctionName { get; set; }
        public int NumberOfBidding { get; set; }
        public double? HighestBidding { get; set; }
        public int HighestBiddingPersonId { get; set; }
    }
}
