using API.DTOs.Requests.Posts;
using API.DTOs.Requests.Properties;
using API.DTOs.Responses.Posts;
using API.DTOs.Responses.Properties;
using API.Services.Interfaces;
using AutoMapper;
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
        //private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PropertyService(IRepositoryBase<Property> propertyRepository, IMapper mapper, IRepositoryBase<Post> postRepository)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            //_postService = postService;
            _postRepository = postRepository;
        }

        public async Task<List<GetPropertyResponse>> Get()
        {
            var result = await _propertyRepository.GetAsync(navigationProperties: new string[]
                { "Author", "Post"});
            var response = _mapper.Map<List<GetPropertyResponse>>(result);
            return response;
        }

        public async Task<GetPropertyResponse> GetById(int id)
        {
            var result =
                await _propertyRepository.FoundOrThrow(u => u.Id.Equals(id), new KeyNotFoundException("Property is not exist"));
            var entity = _mapper.Map(result, new GetPropertyResponse());
            DataResponse.CleanNullableDateTime(entity);
            return entity;
        }

        public async Task<Property> CreateProperty(int postId,CreatePropertyRequest model)
        {
            var post = await _postRepository.FirstOrDefaultAsync(x => x.Id.Equals(postId)) ??   
                     throw new KeyNotFoundException();           
            //await _postService.ModifyPostStatus(postId, PostStatus.Completed);
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
            await _propertyRepository.CreateAsync(entity);
            return entity;
        }


        public async Task<Property> UpdateProperty(int id, UpdatePropertyRequest model)
        {
            var target =
                await _propertyRepository.FirstOrDefaultAsync(x => x.Id.Equals(id)) ?? throw new KeyNotFoundException();
            var entity = _mapper.Map(model, target);
            await _propertyRepository.UpdateAsync(entity);
            return entity;
        }

        public async Task Remove(int id)
        {
            var target = await _propertyRepository.FirstOrDefaultAsync(x => x.Id.Equals(id)) ??
                         throw new KeyNotFoundException("Property is not exist");
            await _propertyRepository.DeleteAsync(target);
        }
    }
}
