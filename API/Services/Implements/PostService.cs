using API.DTOs.Requests.Posts;
using API.DTOs.Responses.Posts;
using API.DTOs.Responses.Users;
using API.Services.Interfaces;
using AutoMapper;
using Domain.Constants.Enums;
using Domain.Models;
using Microsoft.Extensions.Options;
using Persistence.Helpers;
using Persistence.Repositories.Interfaces;
using System.Net.Sockets;

namespace API.Services.Implements
{
    public class PostService : IPostService
    {
        private readonly IRepositoryBase<Post> _postRepository;
        //private readonly 
        private readonly IMapper _mapper;

        public PostService(IRepositoryBase<Post> postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;           
        }

        public async Task<List<GetPostResponse>> Get()
        {
            var result = await _postRepository.GetAsync(navigationProperties: new string[]
                { "Author", "Approver"});
            var response = _mapper.Map<List<GetPostResponse>>(result);
            return response;
        }

        public async Task<GetPostResponse> GetById(int id)
        {
            var result =
                await _postRepository.FoundOrThrow(u => u.Id.Equals(id), new KeyNotFoundException("Post is not exist"));
            var entity = _mapper.Map(result, new GetPostResponse());
            DataResponse.CleanNullableDateTime(entity);
            return entity;
        }

        public async Task<List<GetPostResponse>> GetByUser(int userId)
        {
            var result = await _postRepository.WhereAsync(x => x.AuthorId.Equals(userId),
                new string[] { "Author", "Approver" });
            var response = _mapper.Map<List<GetPostResponse>>(result);
            return response;
        }

        public async Task<Post> CreatePostByMember(int createdById, CreatePostRequest model)
        {
            Post entity = _mapper.Map(model, new Post());
            entity.AuthorId = createdById;
            entity.PostStatus = PostStatus.Requesting;
            await _postRepository.CreateAsync(entity);
            return entity;
        }

        public async Task<Post> UpdateByMember(int id, UpdatePostRequest model)
        {
            var target =
                await _postRepository.FirstOrDefaultAsync(x => x.Id.Equals(id)) ?? throw new KeyNotFoundException();
            var entity = _mapper.Map(model, target);
            await _postRepository.UpdateAsync(entity);
            return entity;
        }

        public async Task Remove(int id)
        {
            var target = await _postRepository.FirstOrDefaultAsync(x => x.Id.Equals(id)) ??
                         throw new KeyNotFoundException("Ticket is not exist");
            await _postRepository.SoftDeleteAsync(target);
        }


    }
}
