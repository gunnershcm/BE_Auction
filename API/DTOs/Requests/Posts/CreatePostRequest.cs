using API.DTOs.Requests.Properties;
using API.Mappings;
using Domain.Constants.Enums;
using Domain.Models;

namespace API.DTOs.Requests.Posts
{
    public class CreatePostRequest : IMapTo<Post>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string PropertyName { get; set; }

        public List<string>? PropertyImages { get; set; }

        public string PropertyStreet { get; set; }

        public string PropertyWard { get; set; }

        public string PropertyDistrict { get; set; }

        public string PropertyCity { get; set; }

        public double PropertyArea { get; set; }

        public double PropertyRevervePrice { get; set; }
    }
}
