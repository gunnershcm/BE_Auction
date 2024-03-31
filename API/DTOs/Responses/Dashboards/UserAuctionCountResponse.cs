using API.DTOs.Responses.Auctions;
using API.DTOs.Responses.UserAuctions;
using API.Mappings;
using Domain.Models;

namespace API.DTOs.Responses.Dashboards
{
    public class UserAuctionCountResponse
    {
        public int AuctionId { get; set; }  

        public string? AuctionName { get; set; }

        //public List<GetAuctionForDashboardResponse> Auction { get; set; } 

        public int NumberOfUser { get; set; }

    }


}
