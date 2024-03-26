using API.DTOs.Requests.Posts;
using API.DTOs.Requests.Properties;
using API.DTOs.Responses.Posts;
using API.DTOs.Responses.TransferForms;
using API.DTOs.Responses.Users;
using API.Services.Interfaces;
using AutoMapper;
using Domain.Constants;
using Domain.Constants.Enums;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.Extensions.Options;
using Persistence.Helpers;
using Persistence.Repositories.Interfaces;
using System.Net.Sockets;
using static Grpc.Core.Metadata;

namespace API.Services.Implements
{
    public class TransferFormService : ITransferFormService
    {
        private readonly IRepositoryBase<TransferForm> _transferFormRepository;
        private readonly IRepositoryBase<Property> _propertyRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyService _propertyService;
        private readonly IUrlResourceService _urlResourceService;

        public TransferFormService(IRepositoryBase<TransferForm> transferFormRepository, IMapper mapper,
            IPropertyService propertyService, IUrlResourceService urlResourceService,
            IRepositoryBase<Property> propertyRepository)
        {
            _transferFormRepository = transferFormRepository;
            _mapper = mapper;
            _propertyService = propertyService;
            _urlResourceService = urlResourceService;
            _propertyRepository = propertyRepository;
        }

        public async Task<List<GetTransferFormResponse>> Get()
        {
            var result = await _transferFormRepository.GetAsync(navigationProperties: new string[]
                { "Approver", "Author", "Property"});
            var response = _mapper.Map<List<GetTransferFormResponse>>(result);
            foreach (var entity in response)
            {
                entity.TransferImages = (await _urlResourceService.Get(Tables.TRANSFERFORM, entity.Id, ResourceType.Common)).Select(x => x.Url).ToList();
                entity.TransactionImages = (await _urlResourceService.Get(Tables.TRANSFERFORM, entity.Id, ResourceType.Confirm)).Select(x => x.Url).ToList();
            }
            return response;
        }

        public async Task<GetTransferFormResponse> GetById(int formId)
        {
            var result =
               await _transferFormRepository.FirstOrDefaultAsync(u => u.Id.Equals(formId), new string[]
               { "Approver", "Author", "Property"}) ?? throw new KeyNotFoundException("Form is not exist");
            var entity = _mapper.Map(result, new GetTransferFormResponse());
            entity.TransferImages = (await _urlResourceService.Get(Tables.TRANSFERFORM, entity.Id, ResourceType.Common)).Select(x => x.Url).ToList();
            entity.TransactionImages = (await _urlResourceService.Get(Tables.TRANSFERFORM, entity.Id, ResourceType.Confirm)).Select(x => x.Url).ToList();
            return entity;
        }

        public async Task<List<GetTransferFormResponse>> GetByUser(int userId)
        {
            var result = await _transferFormRepository.WhereAsync(x => x.AuthorId.Equals(userId),
                new string[] { "Author", "Approver", "Property" });
            var response = _mapper.Map<List<GetTransferFormResponse>>(result);
            foreach (var entity in response)
            {
                entity.TransferImages = (await _urlResourceService.Get(Tables.TRANSFERFORM, entity.Id, ResourceType.Common)).Select(x => x.Url).ToList();
                entity.TransactionImages = (await _urlResourceService.Get(Tables.TRANSFERFORM, entity.Id, ResourceType.Confirm)).Select(x => x.Url).ToList();
            }
            return response;
        }


        public async Task<List<GetTransferFormResponse>> GetFormApproveByUser(int userId)
        {
            var result = await _transferFormRepository.WhereAsync(x => x.AuthorId.Equals(userId)
            && x.TranferFormStatus == TranferFormStatus.Approved, new string[] { "Author", "Approver", "Property" });
            var response = _mapper.Map<List<GetTransferFormResponse>>(result);
            foreach (var entity in response)
            {
                entity.TransferImages = (await _urlResourceService.Get(Tables.TRANSFERFORM, entity.Id, ResourceType.Common)).Select(x => x.Url).ToList();
                entity.TransactionImages = (await _urlResourceService.Get(Tables.TRANSFERFORM, entity.Id, ResourceType.Confirm)).Select(x => x.Url).ToList();
            }
            return response;
        }

        public async Task<List<GetTransferFormResponse>> GetFormRejectByUser(int userId)
        {
            var result = await _transferFormRepository.WhereAsync(x => x.AuthorId.Equals(userId)
            && x.TranferFormStatus == TranferFormStatus.Rejected, new string[] { "Author", "Approver", "Property" });
            var response = _mapper.Map<List<GetTransferFormResponse>>(result);
            foreach (var entity in response)
            {
                entity.TransferImages = (await _urlResourceService.Get(Tables.TRANSFERFORM, entity.Id, ResourceType.Common)).Select(x => x.Url).ToList();
                entity.TransactionImages = (await _urlResourceService.Get(Tables.TRANSFERFORM, entity.Id, ResourceType.Confirm)).Select(x => x.Url).ToList();
            }
            return response;
        }

        public async Task<TransferForm> CreateFormByMember(int createdById, CreateTransferFormRequest model)
        {
            TransferForm entity = _mapper.Map(model, new TransferForm());
            await _propertyRepository.FoundOrThrow(u => u.Id.Equals(entity.PropertyId) && (u.isDone == true) && (u.AuthorId == createdById),
                new KeyNotFoundException("Property is not exist or not valid"));
            entity.AuthorId = createdById;
            entity.TranferFormStatus = TranferFormStatus.Requesting;
            var result = await _transferFormRepository.CreateAsync(entity);
            if (model.TransferImages != null)
            {
                await _urlResourceService.Add(Tables.TRANSFERFORM, result.Id, model.TransferImages, ResourceType.Common);
            }
            return result;
        }

        public async Task<TransferForm> UpdateByAdmin(int id, UpdateTransferFormRequest model)
        {
            var target =
                await _transferFormRepository.FirstOrDefaultAsync(x => x.Id.Equals(id)) ?? throw new KeyNotFoundException();
            var entity = _mapper.Map(model, target);
            var result = await _transferFormRepository.UpdateAsync(entity);
            if (model.TransactionImages != null)
            {
                await _urlResourceService.Add(Tables.TRANSFERFORM, result.Id, model.TransactionImages, ResourceType.Confirm);
            }
            return result;
        }

        public async Task Approve(int formId)
        {
            var target = await _transferFormRepository.FirstOrDefaultAsync(c => c.Id.Equals(formId)) ??
                         throw new KeyNotFoundException();
            var property = await _propertyRepository.FoundOrThrow(u => u.Id.Equals(target.PropertyId), new KeyNotFoundException("Property is not exist"));
            //property.isAvailable = false;
            target.TranferFormStatus = TranferFormStatus.Approved;
            target.Reason = null;
            await _transferFormRepository.UpdateAsync(target);
        }

        public async Task<TransferForm> Reject(int formId, UpdateRejectReasonForm model)
        {
            var target = await _transferFormRepository.FirstOrDefaultAsync(c => c.Id.Equals(formId)) ??
                         throw new KeyNotFoundException();
            var entity = _mapper.Map(model, target);
            target.TranferFormStatus = TranferFormStatus.Rejected;
            await _transferFormRepository.UpdateAsync(entity);
            return entity;
        }

        public async Task<TransferForm> ModifyFormStatus(int formId, TranferFormStatus newStatus)
        {
            var form = await _transferFormRepository.FirstOrDefaultAsync(c => c.Id.Equals(formId)) ??
                         throw new KeyNotFoundException("Form is not exist");

            switch (form.TranferFormStatus)
            {
                case TranferFormStatus.Draft:
                    if (newStatus == TranferFormStatus.Requesting)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    else if (newStatus == TranferFormStatus.Rejected)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    else if (newStatus == TranferFormStatus.Approved)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    else if (newStatus == TranferFormStatus.Completed)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    break;

                case TranferFormStatus.Requesting:
                    if (newStatus == TranferFormStatus.Draft)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    else if (newStatus == TranferFormStatus.Rejected)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    else if (newStatus == TranferFormStatus.Approved)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    else if (newStatus == TranferFormStatus.Completed)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    break;

                case TranferFormStatus.Rejected:
                    if (newStatus == TranferFormStatus.Draft)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    else if (newStatus == TranferFormStatus.Requesting)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    else if (newStatus == TranferFormStatus.Approved)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    else if (newStatus == TranferFormStatus.Completed)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    break;

                case TranferFormStatus.Approved:
                    if (newStatus == TranferFormStatus.Draft)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    else if (newStatus == TranferFormStatus.Rejected)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    else if (newStatus == TranferFormStatus.Requesting)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    else if (newStatus == TranferFormStatus.Completed)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    break;
                case TranferFormStatus.Completed:
                    if (newStatus == TranferFormStatus.Draft)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    else if (newStatus == TranferFormStatus.Rejected)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    else if (newStatus == TranferFormStatus.Requesting)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    else if (newStatus == TranferFormStatus.Approved)
                    {
                        form.TranferFormStatus = newStatus;
                        await _transferFormRepository.UpdateAsync(form);
                    }
                    break;
                default:
                    throw new BadRequestException();
            }

            return form;
        }


        public async Task Remove(int id)
        {
            var target = await _transferFormRepository.FirstOrDefaultAsync(x => x.Id.Equals(id)) ??
                         throw new KeyNotFoundException("Form is not exist");
            await _transferFormRepository.DeleteAsync(target);
        }
    }
}
