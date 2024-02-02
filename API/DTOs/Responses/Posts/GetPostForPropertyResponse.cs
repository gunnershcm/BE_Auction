using API.Mappings;
using Domain.Constants.Enums;
using Domain.Models;

namespace API.DTOs.Responses.Posts
{
    public class GetPostForPropertyResponse : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string? Response { get; set; }

        public string PropertyName { get; set; }

        public string PropertyTitle { get; set; }

        public string PropertyDescription { get; set; }

        public string PropertyImage { get; set; }

        public string PropertyAddress { get; set; }

        public double PropertyRevervePrice { get; set; }

        public PostStatus PostStatus { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; }

    }
}
