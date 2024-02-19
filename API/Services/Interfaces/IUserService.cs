using API.DTOs.Requests.Users;
using API.DTOs.Responses.Posts;
using API.DTOs.Responses.Users;
using Domain.Models;

namespace API.Services.Interfaces
{
    public interface IUserService
    {       
        Task<List<GetUserResponse>> Get();
        Task<GetUserResponse> GetById(int userId);
        //Task<List<GetUserResponse>> GetByUser(int userId);
        Task<User> Create(CreateUserRequest model);
        Task<User> Update(int id, UpdateUserRequest model);
        Task Remove(int id);
        Task<string> UploadImageFirebase(int userId, IFormFile file);


    }
}
