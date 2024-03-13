using API.Mappings;
using Domain.Models;

namespace API.DTOs.Requests.UserAuctions
{
    public class BiddingAmountRequest : IMapTo<UserAuction>
    {
        public double BiddingAmount { get; set; }

        public int UserId { get; set; }

        public int AuctionId { get; set; }

        public bool isJoin { get; set; }

    }
}

