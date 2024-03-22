using Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class TransferForm : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string? Reason { get; set; }

        public TranferFormStatus TranferFormStatus { get; set; }

        public int PropertyId { get; set; }
        public Property Property { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; }

        public int? ApproverId { get; set; }
        public User? Approver { get; set; }

    }
}
