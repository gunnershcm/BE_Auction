using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class UserAuction : BaseEntity
    {
        public int? UserId { get; set; }

        public int AuctionId { get; set; }

        public User? User { get; set; }

        public Auction? Auction { get; set; }

        public bool isJoin { get; set; }

    }
}
