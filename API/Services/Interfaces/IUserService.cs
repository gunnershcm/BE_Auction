using API.DTOs.Requests.Users;
using API.DTOs.Responses.Posts;
using API.DTOs.Responses.Users;
using Domain.Models;

namespace API.Services.Interfaces
{
    public interface IUserService
    {       
        Task<List<GetUserResponse>> Get();
        Task<GetUserResponse> GetById(int id);
        Task<List<GetPostResponse>> GetByUser(int userId);
        Task<User> Create(CreateUserRequest model);
        Task Remove(int id);
        Task<User> Update(int id, UpdateUserRequest model);
    }
}
