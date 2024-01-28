using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Notification : BaseEntity
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
        public bool? IsRead { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
