using API.Mappings;
using Domain.Models;

namespace API.DTOs.Requests.Properties
{
    public class UpdatePropertyRequest : IMapTo<Property>
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string Address { get; set; }

        public double RevervePrice { get; set; }
    }
}
    