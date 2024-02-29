using Domain.Models;
using Persistence.Repositories.Interfaces;

namespace API.Services.Implements
{
    public class UserAuctionService : IUserAuctionService
    {
        private readonly IRepositoryBase<UserAuction> _userAuctionRepository;
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IRepositoryBase<Auction> _auctionRepository;

        public UserAuctionService(IRepositoryBase<UserAuction> userAuctionRepository, IRepositoryBase<Auction> auctionRepository)
        {
            _userAuctionRepository = userAuctionRepository;
            _auctionRepository = auctionRepository;
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
