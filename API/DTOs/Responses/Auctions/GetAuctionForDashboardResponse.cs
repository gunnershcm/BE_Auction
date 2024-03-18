using API.DTOs.Responses.Properties;
using API.Mappings;
using Domain.Constants.Enums;
using Domain.Models;

namespace API.DTOs.Responses.Auctions
{
    public class GetAuctionForDashboardResponse : IMapFrom<Auction>
    {
        public int Id { get; set; }

        public string Name { get; set; }

    }
}
