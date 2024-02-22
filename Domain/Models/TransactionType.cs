﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class TransactionType 
    {
        public TransactionType()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<Transaction>? Transactions { get; set; }

    }
}
