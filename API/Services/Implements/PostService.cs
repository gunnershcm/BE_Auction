using API.DTOs.Requests.Posts;
using API.DTOs.Responses.Posts;
using API.Services.Interfaces;
using AutoMapper;
using Domain.Constants.Enums;
using Domain.Models;
using Microsoft.Extensions.Options;
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

        public async Task<Post> CreatePostByMember(int createdById, CreatePostRequest model)
        {
            Post entity = _mapper.Map(model, new Post());
            entity.AuthorId = createdById;
            entity.PostStatus = PostStatus.Requesting;
            //var PropertyId = await 
            await _postRepository.CreateAsync(entity);
            return entity;
        }



    }
}
