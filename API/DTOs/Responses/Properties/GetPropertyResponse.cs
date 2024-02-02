using API.DTOs.Responses.Posts;
using API.Mappings;
using Domain.Models;

namespace API.DTOs.Responses.Properties
{
    public class GetPropertyResponse : IMapFrom<Property>
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string Address { get; set; }

        public double RevervePrice { get; set; }

        public int PostId { get; set; }

        public GetPostForPropertyResponse? Post { get; set; }
    }
}
