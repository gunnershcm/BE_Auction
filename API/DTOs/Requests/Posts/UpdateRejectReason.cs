using API.Mappings;
using Domain.Models;

namespace API.DTOs.Requests.Posts
{
    public class UpdateRejectReason : IMapTo<Post>
    {
        public string Response { get; set; }
    }
}

