using API.Mappings;
using Domain.Constants.Enums;
using Domain.Models;

namespace API.DTOs.Requests.Auths
{
    public class SignUpUser : IMapTo<User>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public Gender Gender { get; set; }

        public string IdentityId { get; set; }

        public DateTime IssuedDate { get; set; }

        public string IssuedPlace { get; set; }

        public string BankNumber { get; set; }

        public int BankId { get; set; }

        public string BankNameHolder { get; set; }
    }
}
