using API.DTOs.Requests.Posts;
using API.DTOs.Requests.Properties;
using API.DTOs.Responses.Posts;
using API.DTOs.Responses.Properties;
using API.Services.Interfaces;
using AutoMapper;
using Domain.Constants;
using Domain.Constants.Enums;
using Domain.Models;
using Persistence.Helpers;
using Persistence.Repositories.Interfaces;

namespace API.Services.Implements
{
    public class PropertyService : IPropertyService
    {
        private readonly IRepositoryBase<Property> _propertyRepository;
        private readonly IRepositoryBase<Post> _postRepository;
        private readonly IUrlResourceService _urlResourceService;
        private readonly IMapper _mapper;

        public PropertyService(IRepositoryBase<Property> propertyRepository, IMapper mapper, 
            IRepositoryBase<Post> postRepository, IUrlResourceService urlResourceService)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _urlResourceService = urlResourceService;
            _postRepository = postRepository;
        }

        public async Task<List<GetPropertyResponse>> Get()
        {
            var result = await _propertyRepository.GetAsync(navigationProperties: new string[]
                { "Author", "Post", "PropertyType"});
            var response = _mapper.Map<List<GetPropertyResponse>>(result);
            foreach (var entity in response)
            {
                entity.Images = (await _urlResourceService.Get(Tables.PROPERTY, entity.Id, ResourceType.Common)).Select(x => x.Url).ToList();
            }
            return response;
        }

        public async Task<GetPropertyResponse> GetById(int id)
        {
            var result =
                await _propertyRepository.FirstOrDefaultAsync(u => u.Id.Equals(id), new string[]
                { "Author", "Post", "PropertyType"}) ?? throw new KeyNotFoundException("Property is not exist");
            var entity = _mapper.Map(result, new GetPropertyResponse());
            entity.Images = (await _urlResourceService.Get(Tables.PROPERTY, entity.Id, ResourceType.Common)).Select(x => x.Url).ToList();
            return entity;
        }

        public async Task<List<GetPropertyResponse>> GetForTransferForm(int userId)
        {
            var result = await _propertyRepository.GetAsync(
                filter: p => p.AuthorId == userId && p.isDone == true,
                navigationProperties: new string[] { "Author", "Post", "PropertyType" }) ?? throw new KeyNotFoundException("Property is not exist");

            var response = _mapper.Map<List<GetPropertyResponse>>(result);

            foreach (var entity in response)
            {
                entity.Images = (await _urlResourceService.Get(Tables.PROPERTY, entity.Id, ResourceType.Common)).Select(x => x.Url).ToList();
            }

            return response;
        }


        public async Task<Property> CreateProperty(int postId,CreatePropertyRequest model)
        {
            var post = await _postRepository.FirstOrDefaultAsync(x => x.Id.Equals(postId)) ??   
                     throw new KeyNotFoundException();           
            Property entity = _mapper.Map(model, new Property());
            entity.AuthorId = post.AuthorId;
            entity.PostId = postId;
            entity.Name = post.PropertyName;
            entity.Street = post.PropertyStreet;
            entity.Ward = post.PropertyWard;
            entity.District = post.PropertyDistrict;
            entity.City = post.PropertyCity;
            entity.Area= post.PropertyArea;
            entity.RevervePrice = post.PropertyRevervePrice;
            entity.PropertyTypeId = post.PropertyTypeId;
            entity.Code = CommonService.CreateRandomPropertyCode();
            entity.isAvailable = true;
            entity.isDone = false;
            entity.Price = 0;
            var postImages = await _urlResourceService.GetUrls(Tables.POST, post.Id, ResourceType.Common);
            model.Images = postImages;
            var result = await _propertyRepository.CreateAsync(entity);
            if (model.Images != null)
            {
                await _urlResourceService.Add(Tables.PROPERTY, result.Id, model.Images, ResourceType.Common);
            }
            return result;
        }

        public async Task<Property> UpdateProperty(int id, UpdatePropertyRequest model)
        {
            var target =
                await _propertyRepository.FirstOrDefaultAsync(x => x.Id.Equals(id)) ?? throw new KeyNotFoundException();
            var entity = _mapper.Map(model, target);
            var result = await _propertyRepository.UpdateAsync(entity);
            if (model.Images != null)
            {
                await _urlResourceService.Update(Tables.PROPERTY, result.Id, model.Images, ResourceType.Common);
            }
            return result;
        }

        public async Task Remove(int id)
        {
            var target = await _propertyRepository.FirstOrDefaultAsync(x => x.Id.Equals(id)) ??
                         throw new KeyNotFoundException("Property is not exist");
            await _propertyRepository.DeleteAsync(target);
        }
    }
}
