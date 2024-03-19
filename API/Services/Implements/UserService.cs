using API.DTOs.Requests.Users;
using API.DTOs.Responses.Posts;
using API.DTOs.Responses.Users;
using API.Services.Interfaces;
using AutoMapper;
using Domain.Constants.Enums;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Persistence.Helpers;
using Persistence.Repositories.Interfaces;

namespace API.Services.Implements;

public class UserService : IUserService
{
    private readonly IRepositoryBase<User> _userRepository;
    private readonly IMapper _mapper;
    private readonly IFirebaseService _firebaseService;

    public UserService(IRepositoryBase<User> userRepository, IMapper mapper, IFirebaseService firebaseService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _firebaseService = firebaseService;
    }

    public async Task<List<GetUserResponse>> Get()
    {
        var result = await _userRepository.ToListAsyncAll();
        var response = new List<GetUserResponse>();
        foreach (var user in result)
        {
            var entity = _mapper.Map(user, new GetUserResponse());
            DataResponse.CleanNullableDateTime(entity);
            response.Add(entity);
        }
        return response;
    }


    public async Task<GetUserResponse> GetById(int userId)
    {
        var result =
            await _userRepository.FoundOrThrow(u => u.Id.Equals(userId) == true, new KeyNotFoundException("User is not exist"));
        var entity = _mapper.Map(result, new GetUserResponse());
        DataResponse.CleanNullableDateTime(entity);
        return entity;
    }

    public async Task<List<GetUserResponse>> GetByUser(int userId)
    {
        var result = await _userRepository.WhereAsync(x => x.Id.Equals(userId));
        var response = _mapper.Map<List<GetUserResponse>>(result);
        return response;
    }


    public async Task<User> Create(CreateUserRequest model)
    {
        User entity = _mapper.Map(model, new User());
        var checkDuplicated = await _userRepository.FirstOrDefaultAsync(x => x.Username == entity.Username);
        if (checkDuplicated != null)
        {
            throw new BadRequestException("Username is exist. Please use a different username.");
        }
        var passwordHasher = new PasswordHasher<User>();
        entity.Password = passwordHasher.HashPassword(entity, model.Password);
        entity.IsActive = true;
        await _userRepository.CreateAsync(entity);
        return entity;
    }

    public async Task<User> Update(int id, UpdateUserRequest model)
    {
        var target =
            await _userRepository.FoundOrThrow(c => c.Id.Equals(id), new KeyNotFoundException("User is not exist"));
        User user = _mapper.Map(model, target);
        await _userRepository.UpdateAsync(user);
        return user;
    }

    public async Task Remove(int id)
    {
        var target =
            await _userRepository.FoundOrThrow(c => c.Id.Equals(id), new KeyNotFoundException("User is not exist"));
        if (target.Username == "admin")
        {
            throw new Exception("Can not remove this admin account");
        }
        else
        {
            target.IsActive = false;
            await _userRepository.SoftDeleteAsync(target);
        }       
    }

    public async Task UndoSoftDelete(int id)
    {
        var target =
            await _userRepository.FoundOrThrowAll(c => c.Id.Equals(id), new KeyNotFoundException("User is not exist"));
        target.IsActive = true;
        await _userRepository.UndoSoftDeleteAsync(target);
    }



    public async Task<string> UploadImageFirebase(int userId, IFormFile file)
    {
        var user = await _userRepository.FoundOrThrow(c => c.Id.Equals(userId),
            new KeyNotFoundException("User is not exist"));
        if (file == null || file.Length == 0)
        {
            throw new BadRequestException("No file uploaded.");
        }
        var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Position = 0;
        var linkImage = await _firebaseService.UploadFirebaseAsync(stream, file.FileName);
        user.AvatarUrl = linkImage;
        await _userRepository.UpdateAsync(user);
        return linkImage;
    }
    
}