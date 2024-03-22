using API.DTOs.Responses.Dashboards;
using API.Services.Interfaces;
using AutoMapper;
using Domain.Constants.Enums;
using Domain.Exceptions;
using Domain.Models;
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

        public DashboardService(IRepositoryBase<Auction> auctionRepository, 
            IRepositoryBase<UserAuction> userAuctionRepository, IMapper mapper, IAuctionService auctionService) 
        {
            _auctionRepository = auctionRepository;
            _userAuctionRepository = userAuctionRepository;
            _mapper = mapper;
            _auctionService = auctionService;
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

    }
}
