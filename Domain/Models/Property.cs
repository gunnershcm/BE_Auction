using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Property : BaseEntity
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string Address { get; set; }

        public double RevervePrice { get; set; }

        public int AuthorId { get; set; }

        public User? Author { get; set; }

        
    }
}
