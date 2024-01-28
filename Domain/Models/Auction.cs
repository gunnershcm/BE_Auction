using System;
using System.Collections.Generic;
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

        public string Name { get; set; }

        public DateTime OpenTime { get; set; }

        public DateTime EndTime { get; set; }

        public double RevervePrice { get; set; }

        public double JoiningFee { get; set; }

        public double StepFee { get; set; }

        public double Deposit { get; set; }

        public string Method { get; set; }

        public DateTime BiddingStartTime { get; set; }

        public DateTime BiddingEndTime { get; set;}

        public double FinalPrice { get; set; }

        public int PropertyId { get; set; }

        public Property? Property { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserAuction>? UserAuctions { get; set; }

        [JsonIgnore]
        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}
