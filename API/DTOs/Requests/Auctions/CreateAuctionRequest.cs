using API.Mappings;
using Domain.Models;

namespace API.DTOs.Requests.Auctions
{
    public class CreateAuctionRequest : IMapTo<Auction>
    {
        public string Name { get; set; }

        public DateTime OpenTime { get; set; }

        public DateTime EndTime { get; set; }

        public double RevervePrice { get; set; }

        public double JoiningFee { get; set; }

        public double StepFee { get; set; }

        public double Deposit { get; set; }

        public string Method { get; set; }

        public DateTime BiddingStartTime { get; set; }

        public DateTime BiddingEndTime { get; set; }

        public List<string>? AuctionImages { get; set; }

    }
}
