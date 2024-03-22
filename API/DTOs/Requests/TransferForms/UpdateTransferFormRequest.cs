using API.Mappings;
using Domain.Constants.Enums;
using Domain.Models;

namespace API.DTOs.Responses.TransferForms
{
    public class UpdateTransferFormRequest : IMapTo<TransferForm>
    {
        public List<string>? TransactionImages { get; set; }
     
    }
}
