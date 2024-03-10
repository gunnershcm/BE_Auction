using API.Mappings;
using Domain.Constants.Enums;
using Domain.Models;

namespace API.DTOs.Requests.Auctions
{
    public class CreatePaymentRequest : IMapTo<Transaction>
    {
        public string Description { get; set; }

        public double Amount { get; set; }

        public string PaymentMethod { get; set; }

        public TransactionStatus TransactionStatus { get; set; }

        public int? UserId { get; set; }

        public int AuctionId { get; set; }

        public int TransactionTypeId { get; set; }

    }
}
