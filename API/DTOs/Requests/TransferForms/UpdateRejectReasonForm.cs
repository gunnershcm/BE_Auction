using API.Mappings;
using Domain.Models;

namespace API.DTOs.Requests.Posts
{
    public class UpdateRejectReasonForm : IMapTo<TransferForm>
    {
        public string Reason { get; set; }
    }
}

