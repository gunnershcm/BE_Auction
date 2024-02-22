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

        public string? Reason { get; set; }

        public PostStatus PostStatus { get; set; }

        public string PropertyName { get; set; }

        public string PropertyTitle { get; set; }

        public string PropertyDescription { get; set; }

        public string PropertyImage { get; set; }

        public string PropertyStreet { get; set; }

        public string PropertyWard { get; set; }

        public string PropertyDistrict { get; set; }

        public string PropertyCity { get; set; }

        public double Area { get; set; }

        public double PropertyRevervePrice { get; set; }    

        public int AuthorId { get; set; }
        public User Author { get; set; }

        public int? ApproverId { get; set; }
        public User? Approver { get; set; }      

    }
}
