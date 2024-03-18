using API.DTOs.Requests.Auctions;
using API.DTOs.Responses.Auctions;
using Domain.Constants.Enums;
using Domain.Models;

namespace API.Services.Interfaces
{
    public interface IAuctionService
    {
        Task<List<GetAuctionResponse>> Get();
        Task<List<GetAuctionForDashboardResponse>> GetAuctionsByMonth(DateTime startOfMonth, DateTime endOfMonth);
        Task<GetAuctionResponse> GetById(int id);
        Task<Auction> CreateAuctionByStaff(CreateAuctionRequest model);
        Task<Auction> UpdateByStaff(int id, UpdateAuctionRequest model);
        Task<Auction> ModifyAuctionStatus(int auctionId, AuctionStatus newStatus);
        Task Remove(int id);
    }
}
