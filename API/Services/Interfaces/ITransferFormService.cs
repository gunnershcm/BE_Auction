using API.DTOs.Requests.Posts;
using API.DTOs.Responses.TransferForms;
using Domain.Constants.Enums;
using Domain.Models;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface ITransferFormService
    {
        Task<List<GetTransferFormResponse>> Get();
        Task<GetTransferFormResponse> GetById(int formId);
        Task<List<GetTransferFormResponse>> GetByUser(int userId);
        Task<List<GetTransferFormResponse>> GetFormApproveByUser(int userId);
        Task<List<GetTransferFormResponse>> GetFormRejectByUser(int userId);
        Task<TransferForm> CreateFormByMember(int createdById, CreateTransferFormRequest model);
        Task<TransferForm> UpdateByAdmin(int id, UpdateTransferFormRequest model);
        Task Approve(int formId);
        Task<TransferForm> Reject(int formId, UpdateRejectReasonForm model);
        Task<TransferForm> ModifyFormStatus(int formId, TranferFormStatus newStatus);
        Task Remove(int id);
    }
}
