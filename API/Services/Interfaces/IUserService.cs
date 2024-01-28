using API.DTOs.Requests.Users;
using API.DTOs.Responses.Users;
using Domain.Models;

namespace API.Services.Interfaces
{
    public interface IUserService
    {       
        Task<List<GetUserResponse>> Get();
        Task<GetUserResponse> GetById(int id);
        Task<User> Create(CreateUserRequest model);
        Task Remove(int id);
    }
}
