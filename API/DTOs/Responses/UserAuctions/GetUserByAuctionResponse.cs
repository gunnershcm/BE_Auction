using API.Mappings;
using Domain.Models;

namespace API.DTOs.Responses.UserAuctions
{
    public class GetUserByAuctionResponse : IMapFrom<UserAuction>
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public User? User { get; set; }

        public bool isJoin { get; set; }

        public double? BiddingAmount { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

    }
}
