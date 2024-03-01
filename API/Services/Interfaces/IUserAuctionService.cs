using API.DTOs.Responses.UserAuctions;

namespace API.Services.Implements
{
    public interface IUserAuctionService
    {
        Task JoinAuction(int userId, int auctionId);
        Task<List<GetUserAuctionResponse>> Get();
        Task<GetUserAuctionResponse> GetById(int id);
    }
}
