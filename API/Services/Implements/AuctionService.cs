﻿using API.DTOs.Requests.Posts;
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

namespace API.Services.Implements
{
    public class AuctionService : IAuctionService
    {
        private readonly IRepositoryBase<Auction> _auctionRepository;
        private readonly IRepositoryBase<Property> _propertyRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyService _propertyService;
        private readonly IUrlResourceService _urlResourceService;
        private readonly IRepositoryBase<Post> _postRepository;

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
                await _auctionRepository.FirstOrDefaultAsync(u => u.Id.Equals(id), new string[]
                {"Property"}) ?? throw new KeyNotFoundException("Auction is not exist");
            var entity = _mapper.Map(result, new GetAuctionResponse());
            entity.AuctionImages = (await _urlResourceService.Get(Tables.AUCTION, entity.Id)).Select(x => x.Url).ToList();
            return entity;
        }
       
        public async Task<Auction> CreateAuctionByStaff(CreateAuctionRequest model)
        {
            Auction entity = _mapper.Map(model, new Auction());
            var property = await _propertyRepository.FoundOrThrow(u => u.Id.Equals(entity.PropertyId), new KeyNotFoundException("Property is not exist"));
            property.isAvailable = false;
            entity.AuctionStatus = AuctionStatus.ComingUp;
            var propertyImages = await _urlResourceService.GetUrls(Tables.PROPERTY, property.Id);
            model.AuctionImages = propertyImages;
            model.FinalPrice = 0;
            model.JoiningFee = 50000;
            model.StepFee = 0.1 * (model.RevervePrice);
            var result = await _auctionRepository.CreateAsync(entity);
            if (model.AuctionImages != null)
            {
                await _urlResourceService.Add(Tables.AUCTION, result.Id, model.AuctionImages);
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
                    break;

               
                default:
                    throw new BadRequestException();
            }

            return auction;
        }
    }
}
