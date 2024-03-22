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
        private readonly IRepositoryBase<UserAuction> _userAuctionRepository;
        private readonly IRepositoryBase<Property> _propertyRepository;
        private readonly IMapper _mapper;
        

        public PaymentService(IRepositoryBase<Auction> auctionRepository, IMapper mapper,
            IRepositoryBase<Transaction> paymentRepository, IRepositoryBase<TransactionType> tranTypeRepository, 
            IRepositoryBase<UserAuction> userAuctionRepository, IRepositoryBase<Property> propertyRepository)
        {
            _auctionRepository = auctionRepository;
            _mapper = mapper;
            _paymentRepository = paymentRepository;
            _tranTypeRepository = tranTypeRepository;
            _userAuctionRepository = userAuctionRepository;
            _propertyRepository = propertyRepository;
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

        public async Task<List<GetPaymentResponse>> GetPaymentAvailable(int userId)
        {
            var result = await _paymentRepository.WhereAsync(x => x.UserId.Equals(userId),
               new string[] { "Auction", "TransactionType" });
            var response = _mapper.Map<List<GetPaymentResponse>>(result);
            return response;
        }


        public async Task PayJoiningFeeAuction(int userId, int auctionId)                   
        {                                                                                   
            await _auctionRepository.FoundOrThrow(u => u.Id.Equals(auctionId), new KeyNotFoundException("Auction is not exist"));
            var transactionType =  await _tranTypeRepository.FirstOrDefaultAsync(u => u.Name.Equals("JoiningFee"));
            var target = await _paymentRepository.FirstOrDefaultAsync(u => u.UserId.Equals(userId) &&
            u.AuctionId.Equals(auctionId) && u.TransactionTypeId.Equals(transactionType.Id));
            if (target != null)
            {
                throw new InvalidOperationException("You has already paid joining fee for this auction");
            }
            Transaction transaction = new Transaction();
            transaction.UserId = userId;
            transaction.AuctionId = auctionId;
            transaction.TransactionTypeId = transactionType.Id;
            transaction.TransactionStatus = TransactionStatus.Paid;
            transaction.Amount = 50000;
            await _paymentRepository.CreateAsync(transaction);
        }

        public async Task PayDepositFeeAuction(int userId, int auctionId)
        {
            var auction = await _auctionRepository.FoundOrThrow(u => u.Id.Equals(auctionId), new KeyNotFoundException("Auction is not exist"));
            var property = await _propertyRepository.FoundOrThrow(u => u.Id.Equals(auction.PropertyId), new KeyNotFoundException("Property is not exist"));
            var transactionType = await _tranTypeRepository.FirstOrDefaultAsync(u => u.Name.Equals("Deposit"));
            var userAuction = await _userAuctionRepository.FirstOrDefaultAsync(u => u.UserId.Equals(userId) && u.AuctionId.Equals(auctionId));
            var target = await _paymentRepository.FirstOrDefaultAsync(u => u.UserId.Equals(userId) &&
            u.AuctionId.Equals(auctionId) && u.TransactionTypeId.Equals(transactionType.Id));
            if (target != null)
            {
                throw new InvalidOperationException("You has already paid deposit fee for this auction");
            }
            Transaction transaction = new Transaction();
            transaction.UserId = userId;
            transaction.AuctionId = auctionId;
            transaction.TransactionTypeId = transactionType.Id;
            transaction.TransactionStatus = TransactionStatus.Paid;
            transaction.Amount = 0.1 * (auction.FinalPrice);
            auction.Deposit = transaction.Amount;
            await _paymentRepository.CreateAsync(transaction);
            userAuction.isWin = true;
            await _userAuctionRepository.UpdateAsync(userAuction);
            property.isDone = true;
            await _propertyRepository.UpdateAsync(property);
        }
    }
}
