using API.Mappings;
using Domain.Constants.Enums;
using Domain.Models;

namespace API.DTOs.Requests.Users
{
    public class UpdateUserRequest : IMapTo<User>
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public Role? Role { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public Gender? Gender { get; set; }
    }
}
