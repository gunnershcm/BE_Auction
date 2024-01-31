using API.DTOs.Requests.Posts;
using API.Services.Interfaces;
using AutoMapper;
using Domain.Constants;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories.Interfaces;
using System.Net.Sockets;

namespace API.Controllers
{

    [Route("/v1/auction/post")]
    public class PostController : BaseController
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;

        }

        //[Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPost()
        {
            var result = await _postService.Get();
            return Ok(result);
        }

        [Authorize(Roles = Roles.MEMBER)]
        [HttpPost("member/new")]
        public async Task<IActionResult> CreatePostByMember([FromBody] CreatePostRequest model)
        {
            try
            {
                Post entity = await _postService.CreatePostByMember(CurrentUserID, model);  
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }          
        }

    }


}
