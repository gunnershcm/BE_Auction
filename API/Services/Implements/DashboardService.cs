using API.DTOs.Responses.Dashboards;
using API.Services.Interfaces;
using AutoMapper;
using Domain.Constants.Enums;
using Domain.Exceptions;
using Domain.Models;
using Persistence.Repositories;
using Persistence.Repositories.Interfaces;
using System.Linq;

namespace API.Services.Implements
{
    public class DashboardService : IDashboardService
    {
        private readonly IRepositoryBase<Auction> _auctionRepository;
        private readonly IRepositoryBase<UserAuction> _userAuctionRepository;
        private readonly IAuctionService _auctionService;
        private readonly IMapper _mapper;
        private readonly IRepositoryBase<Transaction> _transactionRepository;
        private readonly IRepositoryBase<AuctionHistory> _auctionHistoryRepository;

        public DashboardService(IRepositoryBase<Auction> auctionRepository,
            IRepositoryBase<UserAuction> userAuctionRepository, IMapper mapper,
            IAuctionService auctionService, IRepositoryBase<Transaction> transactionRepository,
            IRepositoryBase<AuctionHistory> auctionHistoryRepository)
        {
            _auctionRepository = auctionRepository;
            _userAuctionRepository = userAuctionRepository;
            _mapper = mapper;
            _auctionService = auctionService;
            _transactionRepository = transactionRepository;
            _auctionHistoryRepository = auctionHistoryRepository;
        }


        public async Task<AuctionDashboardResponse> GetNumberOfAuctionState()
        {
            AuctionDashboardResponse model = new AuctionDashboardResponse();
            try
            {
                model.TotalAuction = (await _auctionRepository.ToListAsync()).Count;
                model.ComingUpAuction = (await _auctionRepository.WhereAsync(x => x.AuctionStatus.Equals(AuctionStatus.ComingUp))).Count;
                model.InProgressAuction = (await _auctionRepository.WhereAsync(x => x.AuctionStatus.Equals(AuctionStatus.InProgress))).Count;
                model.FinishedAuction = (await _auctionRepository.WhereAsync(x => x.AuctionStatus.Equals(AuctionStatus.Finished))).Count;
                model.SucceededAuction = (await _auctionRepository.WhereAsync(x => x.AuctionStatus.Equals(AuctionStatus.Succeeded))).Count;
                model.FailedAuction = (await _auctionRepository.WhereAsync(x => x.AuctionStatus.Equals(AuctionStatus.Failed))).Count;
            }
            catch (Exception ex)
            {
                throw new ServerFailureException(ex.Message);
            }
            return model;
        }



        public async Task<List<UserAuctionCountResponse>> GetUserForAuctionDashBoardByMonth(DateTime currentDate)
        {
            DateTime startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            var userAuctionsInMonth = await _userAuctionRepository
                .WhereAsync(u => u.CreatedAt >= startOfMonth && u.CreatedAt <= endOfMonth);
            var allAuctions = await _auctionService.GetAuctionsByMonth(startOfMonth, endOfMonth);
            var response = allAuctions.Select((auction, index) => new UserAuctionCountResponse
            {
                AuctionId = auction.Id,
                AuctionName = auction.Name,
                NumberOfUser = userAuctionsInMonth.Count(u => u.AuctionId == auction.Id)
            }).ToList();
            return response;
        }

        public async Task<List<TransactionDashboardResponse>> GetTransactionDashBoardInCurrentYear(DateTime currentDate)
        {
            var response = new List<TransactionDashboardResponse>();

            for (int month = 1; month <= 12; month++)
            {
                DateTime startOfMonth = new DateTime(currentDate.Year, month, 1);
                DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                var auctions = await _auctionRepository
                    .WhereAsync(a => a.CreatedAt >= startOfMonth && a.CreatedAt <= endOfMonth);

                var totalAuctions = auctions.Count();

                var totalTransactionAmount = 0.0;
                var totalTransactions = 0;
                var totalUsers = 0;

                foreach (var auction in auctions)
                {
                    var transactions = await _transactionRepository
                        .WhereAsync(t => t.AuctionId == auction.Id && t.CreatedAt >= startOfMonth && t.CreatedAt <= endOfMonth,
                        navigationProperties: new string[] { "User" });

                    totalTransactionAmount += transactions.Sum(t => t.Amount);
                    totalTransactions += transactions.Count;
                    totalUsers += transactions.Select(t => t.UserId).Distinct().Count();
                }

                var transactionResponse = new TransactionDashboardResponse
                {
                    Month = startOfMonth.ToString("MMMM yyyy"),
                    TotalAuction = totalAuctions,
                    NumberOfTransaction = totalTransactions,
                    TotalTransactionAmount = totalTransactionAmount,
                    NumberOfUsers = totalUsers
                };

                response.Add(transactionResponse);
            }

            return response;
        }

        public async Task<List<HistoryBiddingDashboardResponse>> GetBiddingInformationDashboard(DateTime currentDate)
        {
            var response = new List<HistoryBiddingDashboardResponse>();

            DateTime startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var auctions = await _auctionRepository.WhereAsync(a => a.CreatedAt >= startOfMonth && a.CreatedAt <= endOfMonth);

            foreach (var auction in auctions)
            {
                // Lấy thông tin về các lượt đấu giá trong phiên đấu giá này
                var biddings = await _auctionHistoryRepository
                    .WhereAsync(t => t.AuctionId == auction.Id);

                // Số lượng lượt đấu giá
                var numberOfBidding = biddings.Count();

                // Lượt đấu giá cao nhất và người đấu giá cao nhất
                var highestBidding = biddings.Max(t => t.BiddingAmount);
                var highestBiddingPersonId = biddings.FirstOrDefault(t => t.BiddingAmount == highestBidding)?.UserId ?? 0;

                // Tạo đối tượng HistoryBiddingDashboardResponse và thêm vào danh sách response
                var biddingInfo = new HistoryBiddingDashboardResponse
                {
                    AuctionId = auction.Id,
                    AuctionName = auction.Name,
                    NumberOfBidding = numberOfBidding,
                    HighestBidding = highestBidding,
                    HighestBiddingPersonId = highestBiddingPersonId
                };

                response.Add(biddingInfo);
            }
            return response;
        }

    }




}

