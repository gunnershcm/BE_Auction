namespace API.DTOs.Responses.Dashboards
{
    public class TransactionDashboardResponse
    {
        public string Month { get; set; }
        public int TotalAuction { get; set; }
        public int NumberOfTransaction { get; set; }
        public double TotalTransactionAmount { get; set; }
        public int? NumberOfUsers { get; set; }
    }
}
