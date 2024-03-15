using API.Mappings;
using Domain.Models;

namespace API.DTOs.Responses.AuctionHistories
{
    public class GetHistoryByUserResponse : IMapFrom<AuctionHistory>
    {
        public int Id { get; set; }

        public int AuctionId { get; set; }

        public Auction? Auction { get; set; }

        public double? BiddingAmount { get; set; }
    }
}
