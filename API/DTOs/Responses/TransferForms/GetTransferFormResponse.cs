using API.Mappings;
using Domain.Constants.Enums;
using Domain.Models;

namespace API.DTOs.Responses.TransferForms
{
    public class GetTransferFormResponse : IMapFrom<TransferForm>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string? Reason { get; set; }

        public List<string>? TransferImages { get; set; }

        public List<string>? TransactionImages { get; set; }

        public TranferFormStatus TranferFormStatus { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; }

        public int? ApproverId { get; set; }
        public User? Approver { get; set; }

        public int PropertyId { get; set; }
        public Property Property { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
       
    }
}
