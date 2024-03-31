using API.Mappings;
using Domain.Models;

namespace API.DTOs.Responses.UserAuctions
{
    public class GetAuctionByUserResponse : IMapFrom<UserAuction>
    {
        public int Id { get; set; }

        public int AuctionId { get; set; }

        public Auction? Auction { get; set; }

        public bool isJoin { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

    }
}
