using API.Mappings;
using Domain.Models;

namespace API.DTOs.Requests.UserAuctions
{
    public class BiddingHistoryRequest : IMapTo<AuctionHistory>
    {
        public double BiddingAmount { get; set; }
    }
}

