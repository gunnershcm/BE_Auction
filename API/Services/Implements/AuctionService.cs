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
using Microsoft.Extensions.Hosting;
using API.DTOs.Responses.Properties;
using API.DTOs.Responses.UserAuctions;

namespace API.Services.Implements
{
    public class AuctionService : IAuctionService
    {
        private readonly IRepositoryBase<Auction> _auctionRepository;
        private readonly IRepositoryBase<Property> _propertyRepository;
        private readonly IRepositoryBase<PropertyType> _propertyTypeRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyService _propertyService;
        private readonly IUrlResourceService _urlResourceService;
        private readonly IRepositoryBase<Post> _postRepository;

        public AuctionService(IRepositoryBase<Auction> auctionRepository, IMapper mapper,
            IPropertyService propertyService, IUrlResourceService urlResourceService,
            IRepositoryBase<Property> propertyRepository, IRepositoryBase<PropertyType> propertyTypeRepository, IRepositoryBase<Post> postRepository)
        {
            _auctionRepository = auctionRepository;
            _mapper = mapper;
            _propertyService = propertyService;
            _urlResourceService = urlResourceService;
            _propertyRepository = propertyRepository;
            _propertyTypeRepository = propertyTypeRepository;
            _postRepository = postRepository;
        }

        public async Task<List<GetAuctionResponse>> Get()
        {
            var auctions = await _auctionRepository.GetAsync(navigationProperties: new string[]
            {"Property","Property.Post","Property.PropertyType"});
            var responses = _mapper.Map<List<GetAuctionResponse>>(auctions);
            foreach (var response in responses)
            {
                response.Property.Post = _mapper.Map<GetPostForPropertyResponse>(response.Property.Post);
            }
            foreach (var entity in responses)
            {
                entity.AuctionImages = (await _urlResourceService.Get(Tables.AUCTION, entity.Id, ResourceType.Common)).Select(x => x.Url).ToList();
            }
            return responses;
        }

        public async Task<List<GetAuctionForDashboardResponse>> GetAuctionsByMonth(DateTime startOfMonth, DateTime endOfMonth)
        {
            var result = await _auctionRepository
                .WhereAsync(u => u.CreatedAt >= startOfMonth && u.CreatedAt <= endOfMonth);
            var response = _mapper.Map<List<GetAuctionForDashboardResponse>>(result);
            return response;
        }

        public async Task<GetAuctionResponse> GetById(int id)
        {
            var result =
                await _auctionRepository.FirstOrDefaultAsync(u => u.Id.Equals(id), new string[]
                {"Property"}) ?? throw new KeyNotFoundException("Auction is not exist");
            var entity = _mapper.Map(result, new GetAuctionResponse());
            entity.AuctionImages = (await _urlResourceService.Get(Tables.AUCTION, entity.Id, ResourceType.Common)).Select(x => x.Url).ToList();
            return entity;
        }

        public async Task<Auction> CreateAuctionByStaff(CreateAuctionRequest model)
        {
            Auction entity = _mapper.Map(model, new Auction());
            var property = await _propertyRepository.FirstOrDefaultAsync(u => u.Id.Equals(entity.PropertyId), new string[] {"Post"}) ?? throw new KeyNotFoundException("Property is not exist or in valid");
            var post = await _postRepository.FoundOrThrow(u => u.Id.Equals(property.PostId), new KeyNotFoundException("Post is not exist"));
            property.isAvailable = false;
            property.isDone = false;
            post.PostStatus = PostStatus.Completed;
            entity.AuctionStatus = AuctionStatus.ComingUp;
            var propertyImages = await _urlResourceService.GetUrls(Tables.PROPERTY, property.Id, ResourceType.Common);
            model.AuctionImages = propertyImages;
            model.FinalPrice = 0;
            model.JoiningFee = 50000;
            model.StepFee = 0.1 * (model.RevervePrice);
            if (1>model.MaxStepFee && model.MaxStepFee> 10)
            {
                throw new InvalidOperationException("MaxStepFee invalid");
            }
            model.Deposit = 0;
            var result = await _auctionRepository.CreateAsync(entity);
            if (model.AuctionImages != null)
            {
                await _urlResourceService.Add(Tables.AUCTION, result.Id, model.AuctionImages, ResourceType.Common);
            }
            return result;
        }

        public async Task<Auction> UpdateByStaff(int id, UpdateAuctionRequest model)
        {
            var target =
                await _auctionRepository.FirstOrDefaultAsync(x => x.Id.Equals(id)) ?? throw new KeyNotFoundException();
            var entity = _mapper.Map(model, target);
            var result = await _auctionRepository.UpdateAsync(entity);
            if (model.AuctionImages != null)
            {
                await _urlResourceService.Update(Tables.AUCTION, result.Id, model.AuctionImages, ResourceType.Common);
            }
            return result;
        }


        public async Task Remove(int id)
        {
            var target = await _auctionRepository.FirstOrDefaultAsync(x => x.Id.Equals(id)) ??
                         throw new KeyNotFoundException("Auction is not exist");
            await _auctionRepository.DeleteAsync(target);
        }


        public async Task<Auction> ModifyAuctionStatus(int auctionId, AuctionStatus newStatus)
        {
            var auction = await _auctionRepository.FirstOrDefaultAsync(c => c.Id.Equals(auctionId)) ??
                         throw new KeyNotFoundException("Auction is not exist");

            switch (auction.AuctionStatus)
            {
                case AuctionStatus.ComingUp:
                    if (newStatus == AuctionStatus.InProgress)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    else if (newStatus == AuctionStatus.Finished)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    else if (newStatus == AuctionStatus.Succeeded)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    else if (newStatus == AuctionStatus.Failed)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    break;

                case AuctionStatus.InProgress:
                    if (newStatus == AuctionStatus.ComingUp)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    else if (newStatus == AuctionStatus.Finished)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    else if (newStatus == AuctionStatus.Succeeded)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    else if (newStatus == AuctionStatus.Failed)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    break;

                case AuctionStatus.Finished:
                    if (newStatus == AuctionStatus.ComingUp)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    else if (newStatus == AuctionStatus.InProgress)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    else if (newStatus == AuctionStatus.Succeeded)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    else if (newStatus == AuctionStatus.Failed)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    break;

                case AuctionStatus.Succeeded:
                    if (newStatus == AuctionStatus.ComingUp)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    else if (newStatus == AuctionStatus.InProgress)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    else if (newStatus == AuctionStatus.Finished)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    else if (newStatus == AuctionStatus.Failed)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    break;
                case AuctionStatus.Failed:
                    if (newStatus == AuctionStatus.ComingUp)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    else if (newStatus == AuctionStatus.InProgress)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    else if (newStatus == AuctionStatus.Finished)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    else if (newStatus == AuctionStatus.Succeeded)
                    {
                        auction.AuctionStatus = newStatus;
                        await _auctionRepository.UpdateAsync(auction);
                    }
                    break;


                default:
                    throw new BadRequestException();
            }

            return auction;
        }
    }
}
