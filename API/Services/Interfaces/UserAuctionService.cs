namespace API.Services.Implements
{
    public interface IUserAuctionService
    {
        Task JoinAuction(int userId, int auctionId);
    }
}
