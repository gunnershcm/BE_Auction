using API.DTOs.Requests.Posts;
using API.DTOs.Responses.Posts;
using API.DTOs.Responses.Users;
using Domain.Models;

namespace API.Services.Interfaces
{
    public interface IPostService
    {
        Task<List<GetPostResponse>> Get();
        Task<GetPostResponse> GetById(int id);
        Task<List<GetPostResponse>> GetByUser(int userId);
        Task<List<GetPostResponse>> GetPostApproveByUser(int userId);
        Task<List<GetPostResponse>> GetPostRejectByUser(int userId);
        Task<Post> CreatePostByMember(int createdById, CreatePostRequest model);
        Task<Post> UpdateByMember(int id, UpdatePostRequest model);
        Task Approve(int postId);
        Task Reject(int postId);
        Task Remove(int id);
    }
}
