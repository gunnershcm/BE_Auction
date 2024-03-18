namespace API.DTOs.Responses.Dashboards
{
    public class AuctionDashboardResponse
    {
        public int TotalAuction { get; set; }
        public int ComingUpAuction { get; set; }
        public int InProgressAuction { get; set; }
        public int FinishedAuction { get; set; }
        public int SucceededAuction { get; set; }
        public int FailedAuction { get; set; }

    }


}
