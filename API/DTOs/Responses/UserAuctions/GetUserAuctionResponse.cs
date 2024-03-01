using API.Mappings;
using Domain.Models;

namespace API.DTOs.Responses.UserAuctions
{
    public class GetUserAuctionResponse : IMapFrom<UserAuction>
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int AuctionId { get; set; }

        public User? User { get; set; }

        public Auction? Auction { get; set; }

        public bool isJoin { get; set; }

    }
}
