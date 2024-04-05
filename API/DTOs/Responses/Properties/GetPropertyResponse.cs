using API.DTOs.Responses.Posts;
using API.Mappings;
using Domain.Models;

namespace API.DTOs.Responses.Properties
{
    public class GetPropertyResponse : IMapFrom<Property>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public List<string>? Images { get; set; }

        public string Street { get; set; }

        public string Ward { get; set; }

        public string District { get; set; }

        public string City { get; set; }

        public double Area { get; set; }

        public double RevervePrice { get; set; }

        public bool isAvailable { get; set; }

        public bool isDone { get; set; }

        public double Price { get; set; }

        public int? AuctionId { get; set; }

        public int PostId { get; set; }

        public GetPostForPropertyResponse? Post { get; set; }

        public int? PropertyTypeId { get; set; }

        public PropertyType? PropertyType { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
