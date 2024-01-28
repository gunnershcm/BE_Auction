using API.Mappings;
using Domain.Constants.Enums;
using Domain.Models;

namespace API.DTOs.Responses.Posts
{
    public class GetPostResponse : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string? Response { get; set; }

        public PostStatus PostStatus { get; set; }

        public int PropertyId { get; set; }

        public Property? Property { get; set; }
    }
}
