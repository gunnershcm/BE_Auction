using API.DTOs.Requests.Posts;
using API.DTOs.Requests.Properties;
using API.DTOs.Responses.Posts;
using API.Services.Implements;
using API.Services.Interfaces;
using Domain.Constants;
using Domain.Constants.Enums;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Helpers;
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
        [ProducesResponseType(typeof(IEnumerable<GetPostResponse>), 200)]
        public async Task<IActionResult> GetAllPost()
        {
            var result = await _postService.Get();
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetPostResponse>), 200)]
        public async Task<IActionResult> GetPosts(
            [FromQuery] string? filter,
            [FromQuery] string? sort,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            try
            {
                var result = await _postService.Get();
                var pagedResponse = result.AsQueryable().GetPagedData(page, pageSize, filter, sort);
                return Ok(pagedResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<GetPostResponse>), 200)]
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
        [ProducesResponseType(typeof(IEnumerable<GetPostResponse>), 200)]
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
        [HttpGet("user/available")]
        [ProducesResponseType(typeof(IEnumerable<GetPostResponse>), 200)]
        public async Task<IActionResult> GetPostByUserAvailable()
        {
            try
            {
                var result = await _postService.GetByUser(CurrentUserID);
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
        [ProducesResponseType(typeof(IEnumerable<GetPostResponse>), 200)]
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
        [ProducesResponseType(typeof(IEnumerable<GetPostResponse>), 200)]
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
        [ProducesResponseType(typeof(IEnumerable<GetPostResponse>), 200)]
        public async Task<IActionResult> CreatePostByMember([FromBody] CreatePostRequest model)
        {
            try
            {
                var entity = await _postService.CreatePostByMember(CurrentUserID, model);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.MEMBER)]
        [HttpPut("member/{postId}")]
        [ProducesResponseType(typeof(IEnumerable<GetPostResponse>), 200)]
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
        [ProducesResponseType(typeof(IEnumerable<GetPostResponse>), 200)]
        public async Task<IActionResult> DeletePost(int postId)
        {
            try
            {
                var post = await _postRepository.FirstOrDefaultAsync(x => x.Id == postId);
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
        [ProducesResponseType(typeof(IEnumerable<GetPostResponse>), 200)]
        public async Task<IActionResult> ApprovePost(int postId)
        {
            try
            {
                await _postService.Approve(postId); 
                return Ok("Approve Post Successfully");
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
        [ProducesResponseType(typeof(IEnumerable<GetPostResponse>), 200)]
        public async Task<IActionResult> RejectPost(int postId, [FromBody] UpdateRejectReason model)
        {
            try
            {
                await _postService.Reject(postId, model);
                return Ok("Reject Post Successfully");
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
        [HttpPatch("modify-status")]
        public async Task<IActionResult> ModifyPostStatus(int postId, PostStatus newStatus)
        {
            try
            {
                var updated = await _postService.ModifyPostStatus(postId, newStatus);
                return Ok("Status Updated Successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }


}
