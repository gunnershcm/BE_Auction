using API.DTOs.Requests.UserAuctions;
using API.DTOs.Responses.AuctionHistories;
using API.DTOs.Responses.UserAuctions;
using API.Services.Interfaces;
using AutoMapper;
using Domain.Constants.Enums;
using Domain.Models;
using Persistence.Helpers;
using Persistence.Repositories.Interfaces;

namespace API.Services.Implements
{
    public class AuctionHistoryService : IAuctionHistoryService
    {
        private readonly IRepositoryBase<AuctionHistory> _auctionHistoryRepository;
        private readonly IRepositoryBase<Auction> _auctionRepository;
        private readonly IMapper _mapper;   

        public AuctionHistoryService(IRepositoryBase<AuctionHistory> auctionHistoryRepository,
            IRepositoryBase<Auction> auctionRepository, IMapper mapper)
        {
            _auctionHistoryRepository = auctionHistoryRepository;
            _auctionRepository = auctionRepository;
            _mapper = mapper;
        }

        public async Task<List<GetAuctionHistoryResponse>> Get()
        {
            var result = await _auctionHistoryRepository.GetAsync(navigationProperties: new string[]
                { "User", "Auction"});
            var response = _mapper.Map<List<GetAuctionHistoryResponse>>(result);
            return response;
        }

        public async Task<GetAuctionHistoryResponse> GetById(int id)
        {
            var result =
                await _auctionHistoryRepository.FirstOrDefaultAsync(u => u.Id.Equals(id), new string[]
                { "User", "Auction"}) ?? throw new KeyNotFoundException("Auction History is not exist");
            var entity = _mapper.Map(result, new GetAuctionHistoryResponse());
            DataResponse.CleanNullableDateTime(entity);
            return entity;
        }



        public async Task<List<GetHistoryByAuctionResponse>> GetHistoryByAuction(int auctionId)
        {
            var result = await _auctionHistoryRepository.WhereAsync(x => x.AuctionId.Equals(auctionId),
                new string[] { "User" });
            result = result.OrderByDescending(x => x.CreatedAt).ToList();
            var response = _mapper.Map<List<GetHistoryByAuctionResponse>>(result);
            return response;
        }

        public async Task<List<GetHistoryByUserResponse>> GetHistoryByUser(int userId)
        {
            var result = await _auctionHistoryRepository.WhereAsync(x => x.UserId.Equals(userId),
                new string[] { "Auction" });
            var response = _mapper.Map<List<GetHistoryByUserResponse>>(result);
            return response;
        }

        

        public async Task<AuctionHistory> CreateAuctionHistory(int userId, int auctionId, BiddingHistoryRequest model)
        {
            AuctionHistory entity = _mapper.Map(model, new AuctionHistory());
            entity.UserId = userId;
            entity.AuctionId = auctionId;
            entity.BiddingAmount = model.BiddingAmount;
            var result = await _auctionHistoryRepository.CreateAsync(entity);
            return result;
        }
    }
}
