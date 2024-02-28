using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Requests.Auths
{
    public class ChangePasswordByEmailRequest
    {
        public string? NewPassword { get; set; }

        public string? ConfirmNewPassword { get; set; }

    }
}
