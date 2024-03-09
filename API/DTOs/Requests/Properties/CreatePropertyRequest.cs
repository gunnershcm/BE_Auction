using API.Mappings;
using Domain.Models;

namespace API.DTOs.Requests.Properties
{
    public class CreatePropertyRequest : IMapTo<Property>
    {
        public string Name { get; set; }

        public List<string>? Images { get; set; }

        public string Street { get; set; }

        public string Ward { get; set; }

        public string District { get; set; }

        public string City { get; set; }

        public double Area { get; set; }

        public double RevervePrice { get; set; }

        public bool isAvailable { get; set; }

    }
}
    