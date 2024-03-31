using API.Mappings;
using Domain.Models;

namespace API.DTOs.Responses.AuctionHistories
{
    public class GetAuctionHistoryResponse : IMapFrom<AuctionHistory>
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int AuctionId { get; set; }

        public Auction? Auction { get; set; }

        public double? BiddingAmount { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
