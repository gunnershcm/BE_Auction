using Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string? Response { get; set; }

        public PostStatus PostStatus { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; }

        public int? ApproverId { get; set; }
        public User? Approver { get; set; }

        public int PropertyId { get; set; }

        public Property? Property { get; set; }

    }
}
