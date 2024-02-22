using Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Transaction : BaseEntity
    {
        public string Description { get; set; }

        public double Amount { get; set; }

        public string PaymentMethod { get; set; }

        public TransactionStatus TransactionStatus { get; set; }

        public int? UserId { get; set; }

        public int AuctionId { get; set; }

        public int TransactionTypeId { get; set; }

        public User? User { get; set; }

        public Auction? Auction { get; set; }

        public TransactionType? TransactionType { get; set;}


    }
}
