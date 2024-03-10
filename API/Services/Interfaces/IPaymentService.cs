using API.DTOs.Responses.Auctions;

namespace API.Services.Implements
{
    public interface IPaymentService
    {
        Task<List<GetPaymentResponse>> Get();
        Task<GetPaymentResponse> GetById(int id);
        Task PayAuction(int userId, int auctionId, int transactionTypeId);
    }
}
