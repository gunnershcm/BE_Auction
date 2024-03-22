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

        public string Code { get; set; }

        public string Street { get; set; }

        public string Ward { get; set; }

        public string District { get; set; }

        public string City { get; set; }

        public double RevervePrice { get; set; }

        public double Area { get; set; }

        public bool isAvailable { get; set; }

        public bool isDone { get; set; }

        public int AuthorId { get; set; }

        public User? Author { get; set; }

        public int PostId { get; set; }

        public Post? Post { get; set; }

        public int? PropertyTypeId { get; set; }

        public PropertyType? PropertyType { get; set; }

    }
}
