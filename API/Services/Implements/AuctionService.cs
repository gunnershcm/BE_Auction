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

namespace API.Services.Implements
{
    public class AuctionService : IAuctionService
    {
        private readonly IRepositoryBase<Auction> _auctionRepository;
        private readonly IRepositoryBase<Property> _propertyRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyService _propertyService;
        private readonly IUrlResourceService _urlResourceService;

        public AuctionService(IRepositoryBase<Auction> auctionRepository, IMapper mapper,
            IPropertyService propertyService, IUrlResourceService urlResourceService, IRepositoryBase<Property> propertyRepository)
        {
            _auctionRepository = auctionRepository;
            _mapper = mapper;
            _propertyService = propertyService;
            _urlResourceService = urlResourceService;
            _propertyRepository = propertyRepository;
        }

        public async Task<List<GetAuctionResponse>> Get()
        {
            var result = await _auctionRepository.GetAsync(navigationProperties: new string[]
                {"Property"});
            var response = _mapper.Map<List<GetAuctionResponse>>(result);
            foreach (var entity in response)
            {
                entity.AuctionImages = (await _urlResourceService.Get(Tables.AUCTION, entity.Id)).Select(x => x.Url).ToList();
            }
            return response;
        }
        
        public async Task<GetAuctionResponse> GetById(int id)
        {
            var result =
                await _auctionRepository.FoundOrThrow(u => u.Id.Equals(id), new KeyNotFoundException("Auction is not exist"));
            var entity = _mapper.Map(result, new GetAuctionResponse());
            entity.AuctionImages = (await _urlResourceService.Get(Tables.AUCTION, entity.Id)).Select(x => x.Url).ToList();
            DataResponse.CleanNullableDateTime(entity);
            return entity;
        }
       
        public async Task<Auction> CreateAuctionByStaff(int propertyId, CreateAuctionRequest model)
        {
            //var auction = await _propertyRepository.FirstOrDefaultAsync(x => x.Id.Equals(propertyId)) ??
            //         throw new KeyNotFoundException();
            Auction entity = _mapper.Map(model, new Auction());
            //entity.AuthorId = post.AuthorId;
            //entity.PostId = postId;
            //entity.Name = post.PropertyName;
            //entity.Street = post.PropertyStreet;
            //entity.Ward = post.PropertyWard;
            //entity.District = post.PropertyDistrict;
            //entity.City = post.PropertyCity;
            //entity.Area = post.PropertyArea;
            //entity.RevervePrice = post.PropertyRevervePrice;
            var result = await _auctionRepository.CreateAsync(entity);
            if (model.AuctionImages != null)
            {
                await _urlResourceService.Add(Tables.PROPERTY, result.Id, model.AuctionImages);
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
                await _urlResourceService.Update(Tables.AUCTION, result.Id, model.AuctionImages);
            }
            return result;
        }

         
        public async Task Remove(int id)
        {
            var target = await _auctionRepository.FirstOrDefaultAsync(x => x.Id.Equals(id)) ??
                         throw new KeyNotFoundException("Auction is not exist");
            await _auctionRepository.DeleteAsync(target);
        }

    }
}
