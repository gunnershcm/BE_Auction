using API.DTOs.Requests.Auctions;
using API.DTOs.Responses.Auctions;
using Domain.Models;

namespace API.Services.Interfaces
{
    public interface IAuctionService
    {
        Task<List<GetAuctionResponse>> Get();
        Task<GetAuctionResponse> GetById(int id);
        Task<Auction> CreateAuctionByStaff(int propertyId, CreateAuctionRequest model);
        Task<Auction> UpdateByStaff(int id, UpdateAuctionRequest model);
        Task Remove(int id);
    }
}
