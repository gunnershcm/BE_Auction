using API.DTOs.Requests.Posts;
using API.DTOs.Requests.Properties;
using API.DTOs.Responses.Posts;
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
    public class PostService : IPostService
    {
        private readonly IRepositoryBase<Post> _postRepository;
        private readonly IRepositoryBase<PropertyType> _propertyTypeRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyService _propertyService;
        private readonly IUrlResourceService _urlResourceService;

        public PostService(IRepositoryBase<Post> postRepository, IMapper mapper,
            IPropertyService propertyService, IUrlResourceService urlResourceService, IRepositoryBase<PropertyType> propertyTypeRepository)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _propertyService = propertyService;
            _urlResourceService = urlResourceService;
            _propertyTypeRepository = propertyTypeRepository;
        }

        public async Task<List<GetPostResponse>> Get()
        {
            var result = await _postRepository.GetAsync(navigationProperties: new string[]
                { "Approver", "PropertyType"});
            var response = _mapper.Map<List<GetPostResponse>>(result);
            foreach (var entity in response)
            {
                entity.PropertyImages = (await _urlResourceService.Get(Tables.POST, entity.Id)).Select(x => x.Url).ToList();
            } 
            return response;
        }

        public async Task<GetPostResponse> GetById(int id)
        {
            var result =
                await _postRepository.FoundOrThrow(u => u.Id.Equals(id), new KeyNotFoundException("Post is not exist"));
            var entity = _mapper.Map(result, new GetPostResponse());
            entity.PropertyImages = (await _urlResourceService.Get(Tables.POST, entity.Id)).Select(x => x.Url).ToList();
            DataResponse.CleanNullableDateTime(entity);
            return entity;
        }

        public async Task<List<GetPostResponse>> GetByUser(int userId)
        {
            var result = await _postRepository.WhereAsync(x => x.AuthorId.Equals(userId),
                new string[] { "Author", "Approver", "PropertyType" });
            var response = _mapper.Map<List<GetPostResponse>>(result);
            foreach (var entity in response)
            {
                entity.PropertyImages = (await _urlResourceService.Get(Tables.POST, entity.Id)).Select(x => x.Url).ToList();
            }
            return response;
        }
   

        public async Task<List<GetPostResponse>> GetPostApproveByUser(int userId)
        {
            var result = await _postRepository.WhereAsync(x => x.AuthorId.Equals(userId)
            && x.PostStatus == PostStatus.Approved, new string[] { "Author", "Approver", "PropertyType" });
            var response = _mapper.Map<List<GetPostResponse>>(result);
            foreach (var entity in response)
            {
                entity.PropertyImages = (await _urlResourceService.Get(Tables.POST, entity.Id)).Select(x => x.Url).ToList();
            }
            return response;
        }

        public async Task<List<GetPostResponse>> GetPostRejectByUser(int userId)
        {
            var result = await _postRepository.WhereAsync(x => x.AuthorId.Equals(userId)
            && x.PostStatus == PostStatus.Rejected, new string[] { "Author", "Approver", "PropertyType" });
            var response = _mapper.Map<List<GetPostResponse>>(result);
            foreach (var entity in response)
            {
                entity.PropertyImages = (await _urlResourceService.Get(Tables.POST, entity.Id)).Select(x => x.Url).ToList();
            }
            return response;
        }

        public async Task<Post> CreatePostByMember(int createdById, CreatePostRequest model)
        {
            Post entity = _mapper.Map(model, new Post());
            await _propertyTypeRepository.FoundOrThrow(u => u.Id.Equals(entity.PropertyTypeId), new KeyNotFoundException("PropertyType is not exist"));
            entity.AuthorId = createdById;
            entity.PostStatus = PostStatus.Requesting;
            var result = await _postRepository.CreateAsync(entity);
            if (model.PropertyImages != null)
            {
                await _urlResourceService.Add(Tables.POST, result.Id, model.PropertyImages);
            }
            return result;
        }

        public async Task<Post> UpdateByMember(int id, UpdatePostRequest model)
        {
            var target =
                await _postRepository.FirstOrDefaultAsync(x => x.Id.Equals(id)) ?? throw new KeyNotFoundException();
            var entity = _mapper.Map(model, target);
            var result = await _postRepository.UpdateAsync(entity);
            if (model.PropertyImages != null)
            {
                await _urlResourceService.Update(Tables.POST, result.Id, model.PropertyImages);
            }
            return result;
        }

        public async Task Approve(int postId)
        {
            var target = await _postRepository.FirstOrDefaultAsync(c => c.Id.Equals(postId)) ??
                         throw new KeyNotFoundException();
            target.PostStatus = PostStatus.Approved;
            target.Reason = null;
            await _postRepository.UpdateAsync(target);
            var model = new CreatePropertyRequest();
            var propertyModel = new CreatePropertyRequest();
            
            await _propertyService.CreateProperty(postId, model);
            
        }

        public async Task<Post> Reject(int postId, UpdateRejectReason model)
        {
            var target = await _postRepository.FirstOrDefaultAsync(c => c.Id.Equals(postId)) ??
                         throw new KeyNotFoundException();
            var entity = _mapper.Map(model, target);
            target.PostStatus = PostStatus.Rejected;
            await _postRepository.UpdateAsync(entity);
            return entity;
        }

        public async Task<Post> ModifyPostStatus(int postId, PostStatus newStatus)
        {
            var post = await _postRepository.FirstOrDefaultAsync(c => c.Id.Equals(postId)) ??
                         throw new KeyNotFoundException("Post is not exist");

            if (post.PostStatus == PostStatus.Completed)
            {
                throw new BadRequestException("Cannot update post status when status is completed");
            }

            if (newStatus == PostStatus.Draft)
            {
                throw new BadRequestException("Cannot update post status to draft");
            }

            switch (post.PostStatus)
            {
                case PostStatus.Draft:
                    if (newStatus == PostStatus.Requesting)
                    {
                        post.PostStatus = newStatus;
                        await _postRepository.UpdateAsync(post);
                    }                  
                    break;
                case PostStatus.Requesting:
                    if (newStatus == PostStatus.Rejected)
                    {
                        post.PostStatus = newStatus;
                        await _postRepository.UpdateAsync(post);
                    }
                    else if (newStatus == PostStatus.Approved)
                    {
                        post.PostStatus = newStatus;
                        await _postRepository.UpdateAsync(post);
                    }
                    break;
              
                case PostStatus.Approved:

                    if (newStatus == PostStatus.Completed)
                    {
                        post.PostStatus = newStatus;
                        await _postRepository.UpdateAsync(post);
                    }

                    break;
                default:
                    throw new BadRequestException();
            }

            return post;
        }


        public async Task Remove(int id)
        {
            var target = await _postRepository.FirstOrDefaultAsync(x => x.Id.Equals(id)) ??
                         throw new KeyNotFoundException("Post is not exist");
            await _postRepository.DeleteAsync(target);
        }


    }
}
