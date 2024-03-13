using API.DTOs.Requests.UserAuctions;
using API.DTOs.Responses.Posts;
using API.DTOs.Responses.UserAuctions;
using API.Services.Interfaces;
using AutoMapper;
using Domain.Constants;
using Domain.Constants.Enums;
using Domain.Models;
using Firebase.Auth;
using Persistence.Helpers;
using Persistence.Repositories.Interfaces;


namespace API.Services.Implements
{
    public class UserAuctionService : IUserAuctionService
    {
        private readonly IRepositoryBase<UserAuction> _userAuctionRepository;
        private readonly IRepositoryBase<Auction> _auctionRepository;
        private readonly IMapper _mapper;

        public UserAuctionService(IRepositoryBase<UserAuction> userAuctionRepository, 
            IRepositoryBase<Auction> auctionRepository, IMapper mapper)
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

        public async Task<List<GetUserByAuctionResponse>> GetUserByAuction(int auctionId)
        {
            var result = await _userAuctionRepository.WhereAsync(x => x.AuctionId.Equals(auctionId),
                new string[] { "User" });
            result = result.OrderByDescending(x => x.BiddingAmount).ToList();
            var response = _mapper.Map<List<GetUserByAuctionResponse>>(result);
            return response;
        }

        public async Task<List<GetAuctionByUserResponse>> GetAuctionByUser(int userId)
        {
            var result = await _userAuctionRepository.WhereAsync(x => x.UserId.Equals(userId),
                new string[] { "Auction" });
            var response = _mapper.Map<List<GetAuctionByUserResponse>>(result);
            return response;
        }

        public async Task<List<GetUserByAuctionResponse>> GetUserTop3ByAuction(int userId)
        {
            var result = await _userAuctionRepository
                .WhereAsync(x => x.UserId.Equals(userId), new string[] { "Auction" });

            var orderedResult = result.OrderByDescending(x => x.BiddingAmount).ToList().Take(3);
            var response = _mapper.Map<List<GetUserByAuctionResponse>>(orderedResult);
            return response;
        }


        public async Task JoinAuction(int userId, int auctionId)
        {
            var auction = await _auctionRepository.FoundOrThrow(u => u.Id.Equals(auctionId), new KeyNotFoundException("Auction is not exist"));
            var target = await _userAuctionRepository.FirstOrDefaultAsync(u => u.UserId.Equals(userId) &&
            u.AuctionId.Equals(auctionId));
            if (target != null)
            {
                throw new InvalidOperationException("You has already joined this auction");
            }else if (auction.AuctionStatus == AuctionStatus.ComingUp)
            {
                UserAuction userAuction = new UserAuction();
                userAuction.UserId = userId;
                userAuction.AuctionId = auctionId;
                userAuction.isJoin = true;
                userAuction.isWin = false;
                await _userAuctionRepository.CreateAsync(userAuction);
            } 
            else
            {
                throw new InvalidOperationException("Invalid Time to join this auction");
            }                
        }
        
        public async Task<UserAuction> BiddingAmount(int userId, int auctionId, BiddingAmountRequest model)
        {
            var target = await _userAuctionRepository.FirstOrDefaultAsync(u => u.UserId.Equals(userId) 
            && u.AuctionId.Equals(auctionId)) ?? throw new KeyNotFoundException("Auction for User is not exist");
            var entity = _mapper.Map(model, target);
            var auction = await _auctionRepository.FirstOrDefaultAsync(a => a.Id.Equals(auctionId));
            if (auction.AuctionStatus != AuctionStatus.InProgress)
            {
                throw new InvalidOperationException("Bidding is not allowed for this auction.");
            }
            else if (model.BiddingAmount < auction.RevervePrice)
            {
                throw new InvalidOperationException("Bidding amount should be greater than ReversePrice.");
            }
            else if (model.BiddingAmount < (auction.FinalPrice + auction.StepFee))
            {
                throw new InvalidOperationException("Bidding amount must be greater than current price with stepFee");
            }
            else
            {
                target.BiddingAmount = model.BiddingAmount;
                auction.FinalPrice = model.BiddingAmount;
                await _auctionRepository.UpdateAsync(auction);
            }
            await _userAuctionRepository.UpdateAsync(entity);
            return entity;
        }

        public async Task<bool> CheckUserJoinedAuction(int userId, int auctionId)
        {
            var userAuction = await _userAuctionRepository.FirstOrDefaultAsync(u =>
                u.UserId.Equals(userId) && u.AuctionId.Equals(auctionId));
            if (userAuction != null)
            {
                return true;
            }
            return false;          
        }
    }
}
