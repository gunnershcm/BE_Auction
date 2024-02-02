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

    public UserService(IRepositoryBase<User> userRepository, IMapper mapper)
    {
        _userRepository = userRepository; 
        _mapper = mapper;
    }

    public async Task<List<GetUserResponse>> Get()
    {
        var result = await _userRepository.ToListAsync();
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
        target.IsActive = false;
        await _userRepository.SoftDeleteAsync(target);
    }

    
}