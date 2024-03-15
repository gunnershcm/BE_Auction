using API.Mappings;
using Domain.Models;

namespace API.DTOs.Responses.AuctionHistories
{
    public class GetHistoryByAuctionResponse : IMapFrom<AuctionHistory>
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public double? BiddingAmount { get; set; }
    }
}
