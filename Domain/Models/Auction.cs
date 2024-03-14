using Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Auction : BaseEntity
    {
        public Auction()
        {
            UserAuctions = new HashSet<UserAuction>();
            Transactions = new HashSet<Transaction>();
        }

        public string Title { get; set; }

        public string? Content { get; set; }

        public string Name { get; set; }

        public AuctionStatus AuctionStatus { get; set; }

        public double RevervePrice { get; set; }

        public double JoiningFee { get; set; }

        public double StepFee { get; set; }

        public double Deposit { get; set; }

        public DateTime BiddingStartTime { get; set; }

        public DateTime BiddingEndTime { get; set; }

        public double FinalPrice { get; set; }

        public int? PropertyId { get; set; }

        [ForeignKey("PropertyId")]
        public Property? Property { get; set; }


        [JsonIgnore]
        public virtual ICollection<UserAuction>? UserAuctions { get; set; }

        [JsonIgnore]
        public virtual ICollection<Transaction>? Transactions { get; set; }

        [JsonIgnore]
        public virtual ICollection<AuctionHistory>? AuctionHistories { get; set; }
    }
}
