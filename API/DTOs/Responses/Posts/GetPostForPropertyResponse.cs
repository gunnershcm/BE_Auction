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

        public string? Reason { get; set; }

        public string PropertyName { get; set; }

        public List<string>? PropertyImages { get; set; }

        public string PropertyStreet { get; set; }

        public string PropertyWard { get; set; }

        public string PropertyDistrict { get; set; }

        public string PropertyCity { get; set; }

        public double PropertyArea { get; set; }

        public double PropertyRevervePrice { get; set; }


        public PostStatus PostStatus { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; }

    }
}
