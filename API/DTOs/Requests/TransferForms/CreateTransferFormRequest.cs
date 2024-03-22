using API.Mappings;
using Domain.Constants.Enums;
using Domain.Models;

namespace API.DTOs.Responses.TransferForms
{
    public class CreateTransferFormRequest : IMapTo<TransferForm>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public List<string>? TransferImages { get; set; }

        public int PropertyId { get; set; }
     
    }
}
