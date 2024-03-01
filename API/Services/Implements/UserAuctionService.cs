using API.DTOs.Responses.Posts;
using API.DTOs.Responses.UserAuctions;
using API.Services.Interfaces;
using AutoMapper;
using Domain.Constants;
using Domain.Models;
using Persistence.Helpers;
using Persistence.Repositories.Interfaces;

namespace API.Services.Implements
{
    public class UserAuctionService : IUserAuctionService
    {
        private readonly IRepositoryBase<UserAuction> _userAuctionRepository;
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IRepositoryBase<Auction> _auctionRepository;
        private readonly IMapper _mapper;

        public UserAuctionService(IRepositoryBase<UserAuction> userAuctionRepository, IRepositoryBase<Auction> auctionRepository, IMapper mapper)
        {
            _userAuctionRepository = userAuctionRepository;
            _auctionRepository = auctionRepository;
            _mapper = mapper;
        }

        public async Task<List<GetUserAuctionResponse>> Get()
        {
            var result = await _userAuctionRepository.GetAsync(navigationProperties: new string[]
                { "User", "Auction"});
            var response = _mapper.Map<List<GetUserAuctionResponse>>(result);
            return response;
        }

        public async Task<GetUserAuctionResponse> GetById(int id)
        {
            var result =
                await _userAuctionRepository.FirstOrDefaultAsync(u => u.Id.Equals(id), new string[]
                { "User", "Auction"}) ?? throw new KeyNotFoundException("User Auction is not exist");
            var entity = _mapper.Map(result, new GetUserAuctionResponse());
            DataResponse.CleanNullableDateTime(entity);
            return entity;
        }

        public async Task JoinAuction(int userId, int auctionId)
        {
            await _auctionRepository.FoundOrThrow(u => u.Id.Equals(auctionId), new KeyNotFoundException("Auction is not exist"));
            UserAuction userAuction = new UserAuction();
            userAuction.UserId = userId;
            userAuction.AuctionId = auctionId;
            userAuction.isJoin = true;
            await _userAuctionRepository.CreateAsync(userAuction);
        }
    }
}
