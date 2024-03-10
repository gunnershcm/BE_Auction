using API.Mappings;
using Domain.Models;

namespace API.DTOs.Requests.Auctions
{
    public class UpdateAuctionRequest : IMapTo<Auction>
    {
        public string Name { get; set; }

        public double RevervePrice { get; set; }

        public double JoiningFee { get; set; }

        public double StepFee { get; set; }

        public double Deposit { get; set; }

        public string Method { get; set; }

        public DateTime BiddingStartTime { get; set; }

        public DateTime BiddingEndTime { get; set; }

        public double FinalPrice { get; set; }

        public List<string>? AuctionImages { get; set; }

        public int PropertyId { get; set; }
    }
}
