using API.DTOs.Requests.Auths;
using API.DTOs.Responses.Auths;
using Domain.Models;

namespace API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest model);
        Task ChangePassword(ChangePasswordRequest model, int userId);
        Task ResetPassword(int uid, string token, ResetPasswordRequest model);
        Task<User> SignUpUser(SignUpUser model);

    }
}