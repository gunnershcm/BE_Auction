﻿using API.DTOs.Requests.Posts;
using API.DTOs.Requests.Properties;
using API.DTOs.Responses.Posts;
using API.DTOs.Responses.Users;
using Domain.Constants.Enums;
using Domain.Models;

namespace API.Services.Interfaces
{
    public interface IPostService
    {
        Task<List<GetPostResponse>> Get();
        Task<GetPostResponse> GetById(int postId);
        Task<List<GetPostResponse>> GetByUser(int userId);
        Task<List<GetPostResponse>> GetPostApproveByUser(int userId);
        Task<List<GetPostResponse>> GetPostRejectByUser(int userId);
        Task<Post> CreatePostByMember(int createdById, CreatePostRequest model);
        Task<Post> UpdateByMember(int id, UpdatePostRequest model);
        Task Approve(int postId);
        Task<Post> Reject(int postId, UpdateRejectReason model);
        Task<Post> ModifyPostStatus(int postId, PostStatus newStatus);
        Task Remove(int id);
    }
}
