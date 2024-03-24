using API.DTOs.Responses.Auctions;

namespace API.Services.Implements
{
    public interface IPaymentService
    {
        Task<List<GetPaymentResponse>> Get();
        Task<GetPaymentResponse> GetById(int id);
        Task<List<GetPaymentResponse>> GetPaymentAvailable(int userId);
        Task PayJoiningFeeAuction(int userId, int auctionId);
        Task PayDepositFeeAuction(int userId, int auctionId);
        Task PayBackDepositFeeAuction(int userId, int auctionId);
        Task Remove(int id);
    }
}
