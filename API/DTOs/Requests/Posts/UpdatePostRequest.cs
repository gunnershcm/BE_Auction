using API.DTOs.Requests.Properties;
using API.Mappings;
using Domain.Constants.Enums;
using Domain.Models;

namespace API.DTOs.Requests.Posts
{
    public class UpdatePostRequest : IMapTo<Post>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string PropertyName { get; set; }

        public string PropertyTitle { get; set; }

        public string PropertyDescription { get; set; }

        public string PropertyImage { get; set; }

        public string PropertyAddress { get; set; }

        public double PropertyRevervePrice { get; set; }
    }
}
