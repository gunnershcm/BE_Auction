namespace API.Services.Interfaces
{
    public interface IHangFireService
    {
        Task UpdateAuctionStatus();
        Task SendMailAuction();
    }
}
