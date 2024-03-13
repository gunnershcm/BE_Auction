using API.Mappings;
using Domain.Models;

namespace API.DTOs.Requests.UserAuctions
{
    public class BiddingAmountRequest : IMapTo<UserAuction>
    {
        public double BiddingAmount { get; set; }
    }
}

