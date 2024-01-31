using Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;

namespace Domain.Models
{
    public class User : BaseEntity
    {
        public User()
        {
            Logs = new HashSet<Log>();
            AuthorPosts = new HashSet<Post>();
            ApproverPosts = new HashSet<Post>();
            Properties = new HashSet<Property>();
            UserAuctions = new HashSet<UserAuction>();
            Transactions = new HashSet<Transaction>();
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public Role Role { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public Gender? Gender { get; set; }

        public string? IdentityId { get; set; }

        public DateTime? IssuedDate { get; set; }

        public string? IssuedPlace { get; set; }

        public string? BankNumber { get; set; }

        public int? BankId { get; set; }

        public string? AccoountHolder { get; set; }

        public bool? IsActive { get; set; }

        public string? AvatarUrl { get; set; }

        public string? PasswordResetToken { get; set; }

        public DateTime? ResetTokenExpires { get; set; }

        [JsonIgnore]
        public virtual ICollection<Log>? Logs { get; set; }

        [JsonIgnore]
        public virtual ICollection<Post>? AuthorPosts { get; set; }

        [JsonIgnore]
        public virtual ICollection<Post>? ApproverPosts { get; set; }

        [JsonIgnore]
        public virtual ICollection<Property>? Properties { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserAuction>? UserAuctions { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<Transaction>? Transactions { get; set; }


    }
}
