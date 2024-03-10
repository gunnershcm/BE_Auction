using API.DTOs.Requests.Posts;
using API.DTOs.Requests.Properties;
using API.DTOs.Responses.Posts;
using API.Services.Interfaces;
using AutoMapper;
using Domain.Constants.Enums;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Persistence.Helpers;
using Persistence.Repositories.Interfaces;
using API.DTOs.Responses.Auctions;
using API.DTOs.Requests.Auctions;
using API.DTOs.Responses.UserAuctions;

namespace API.Services.Implements
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepositoryBase<Auction> _auctionRepository;
        private readonly IRepositoryBase<Transaction> _paymentRepository;
        private readonly IRepositoryBase<TransactionType> _tranTypeRepository;
        private readonly IMapper _mapper;
        

        public PaymentService(IRepositoryBase<Auction> auctionRepository, IMapper mapper,
            IRepositoryBase<Transaction> paymentRepository, IRepositoryBase<TransactionType> tranTypeRepository)
        {
            _auctionRepository = auctionRepository;
            _mapper = mapper;
            _paymentRepository = paymentRepository;
            _tranTypeRepository = tranTypeRepository;
        }

        public async Task<List<GetPaymentResponse>> Get()
        {
            var result = await _paymentRepository.GetAsync(navigationProperties: new string[]
                {"User", "Auction", "TransactionType"});
            var response = _mapper.Map<List<GetPaymentResponse>>(result);         
            return response;
        }


        public async Task<GetPaymentResponse> GetById(int id)
        {
            var result =
                await _paymentRepository.FirstOrDefaultAsync(u => u.Id.Equals(id), new string[]
                {"User", "Auction", "TransactionType"}) ?? throw new KeyNotFoundException("Payment is not exist");
            var entity = _mapper.Map(result, new GetPaymentResponse());
            DataResponse.CleanNullableDateTime(entity);
            return entity;
        }

        public async Task PayAuction(int userId, int auctionId, int transactionTypeId)
        {
            await _auctionRepository.FoundOrThrow(u => u.Id.Equals(auctionId), new KeyNotFoundException("Auction is not exist"));
            await _tranTypeRepository.FoundOrThrow(u => u.Id.Equals(transactionTypeId), new KeyNotFoundException("TransactionType is not exist"));
            var target = await _paymentRepository.FirstOrDefaultAsync(u => u.UserId.Equals(userId) &&
            u.AuctionId.Equals(auctionId) && u.TransactionTypeId.Equals(transactionTypeId));
            if (target != null)
            {
                throw new InvalidOperationException("You has already paid for this auction");
            }
            Transaction transaction = new Transaction();
            transaction.UserId = userId;
            transaction.AuctionId = auctionId;
            transaction.TransactionTypeId = transactionTypeId;
            transaction.TransactionStatus = TransactionStatus.Paid;
            transaction.Amount = 50000;
            await _paymentRepository.CreateAsync(transaction);
        }


    }
}
