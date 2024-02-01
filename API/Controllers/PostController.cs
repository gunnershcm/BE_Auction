using API.DTOs.Requests.Posts;
using API.Services.Implements;
using API.Services.Interfaces;
using AutoMapper;
using Domain.Constants;
using Domain.Exceptions;
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
        private readonly IRepositoryBase<Post> _postRepository;

        public PostController(IPostService postService, IRepositoryBase<Post> postRepository)
        {
            _postService = postService;
            _postRepository = postRepository;
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPost()
        {
            var result = await _postService.Get();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            try
            {
                var result = await _postService.GetById(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPostByUser(int userId)
        {
            try
            {
                var result = await _postService.GetByUser(userId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("approve/user/{userId}")]
        public async Task<IActionResult> GetPostApproveByUser(int userId)
        {
            try
            {
                var result = await _postService.GetPostApproveByUser(userId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("reject/user/{userId}")]
        public async Task<IActionResult> GetPostRejectByUser(int userId)
        {
            try
            {
                var result = await _postService.GetPostRejectByUser(userId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

        [Authorize(Roles = Roles.MEMBER)]
        [HttpPut("member/{postId}")]
        public async Task<IActionResult> UpdatePostByMember(int postId, [FromBody] UpdatePostRequest model)
        {
            try
            {
                var result = await _postService.UpdateByMember(postId, model);
                return Ok("Update Post Successfully");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Post is not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.STAFF)]
        [HttpDelete("staff/{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            try
            {
                var ticket = await _postRepository.FirstOrDefaultAsync(x => x.Id == postId);             
                await _postService.Remove(postId);
                return Ok("Remove Post Successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.STAFF)]
        [HttpPatch("approve")]
        public async Task<IActionResult> ApprovePost(int postId)
        {
            try
            {
                await _postService.Approve(postId);
                return Ok("Approve Successfully");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Post is not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.STAFF)]
        [HttpPatch("reject")]
        public async Task<IActionResult> RejectPost(int postId)
        {
            try
            {
                await _postService.Reject(postId);
                return Ok("Reject Successfully");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Post is not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }


}
