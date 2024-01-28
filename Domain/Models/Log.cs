using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Log : BaseEntity
    {
        public string EntityName { get; set; }

        public int EntityRowId { get; set; }

        public string Action { get; set; }

        public string? Message { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

    }
}
