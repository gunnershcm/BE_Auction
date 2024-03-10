using API.DTOs.Responses.Properties;
using API.Mappings;
using Domain.Constants.Enums;
using Domain.Models;

namespace API.DTOs.Responses.Auctions
{
    public class GetPaymentResponse : IMapFrom<Transaction>
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public double Amount { get; set; }

        public string PaymentMethod { get; set; }

        public TransactionStatus TransactionStatus { get; set; }

        public int? UserId { get; set; }

        public User? User { get; set; }

        public int AuctionId { get; set; }

        public Auction? Auction { get; set; }

        public int TransactionTypeId { get; set; }      

        public TransactionType? TransactionType { get; set; }
    }
}
