using API.DTOs.Responses.Properties;
using API.Mappings;
using Domain.Constants.Enums;
using Domain.Models;

namespace API.DTOs.Responses.Auctions
{
    public class GetAuctionResponse : IMapFrom<Auction>
    {
        public int Id { get; set; }

        public string? Content { get; set; }

        public string Name { get; set; }

        public AuctionStatus AuctionStatus { get; set; }

        public double RevervePrice { get; set; }

        public double JoiningFee { get; set; }

        public double StepFee { get; set; }

        public double Deposit { get; set; }

        public string Method { get; set; }

        public DateTime BiddingStartTime { get; set; }

        public DateTime BiddingEndTime { get; set; }

        public double FinalPrice { get; set; }

        public List<string>? AuctionImages { get; set; }

        public int? PropertyId { get; set; }    
        public GetPropertyResponse? Property { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
