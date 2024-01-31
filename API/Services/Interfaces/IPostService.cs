using API.DTOs.Requests.Posts;
using API.DTOs.Responses.Posts;
using Domain.Models;

namespace API.Services.Interfaces
{
    public interface IPostService
    {
        Task<List<GetPostResponse>> Get();
        Task<Post> CreatePostByMember(int createdById, CreatePostRequest model);
    }
}
