using API.Mappings;
using Domain.Models;

namespace API.DTOs.Responses.UserAuctions
{
    public class GetUserAuctionResponse : IMapFrom<UserAuction>
    {
        public int Id { get; set; }

        public int? UserId { get; set; }  

        public User? User { get; set; }

        public int AuctionId { get; set; }

        public Auction? Auction { get; set; }

        public bool isJoin { get; set; }

        public double? BiddingAmount { get; set; }

        public bool? isWin { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

    }
}
