using API.DTOs.Requests.Auths;
using API.DTOs.Responses.Auths;
using API.Services.Interfaces;
using AutoMapper;
//using Domain.Application.AppConfig;
using Domain.Constants.Enums;
using Domain.Exceptions;
using Domain.Models;
//using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
//using MimeKit;
using Persistence.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace API.Services.Implements;

public class AuthService : IAuthService
{
    private readonly IRepositoryBase<User> _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AuthService(IRepositoryBase<User> userRepository, IConfiguration configuration, IMapper mapper)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<LoginResponse> Login(LoginRequest model)
    {
        var user = await _userRepository.FirstOrDefaultAsync(x => x.Username!.Equals(model.Username)) ?? throw new KeyNotFoundException("User is not found");
        var passwordHasher = new PasswordHasher<User>();
        var isMatchPassword = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password) == PasswordVerificationResult.Success;
        if (!isMatchPassword)
        {
            throw new UnauthorizedException("Password is incorrect");
        }
        if (user.IsActive == false)
        {
            throw new UnauthorizedException("Your account has been suspended");
        }

        var entity = _mapper.Map(user, new LoginResponse());
        entity.AccessToken = GenerateToken(user);
        return entity;
    }


    public async Task ChangePassword(ChangePasswordRequest model, int userId)
    {
        var user = await _userRepository.FoundOrThrow(u => u.Id.Equals(userId), new KeyNotFoundException("User is not found"));

        var passwordHasher = new PasswordHasher<User>();
        var isMatchPassword = passwordHasher.VerifyHashedPassword(user, user.Password, model.CurrentPassword) == PasswordVerificationResult.Success;
        if (!isMatchPassword)
        {
            throw new BadRequestException("Your current password is incorrect.");
        }
        if (model.NewPassword!.Equals(model.CurrentPassword))
        {
            throw new BadRequestException("New password should not be the same as old password.");
        }
        if (!model.NewPassword.Equals(model.ConfirmNewPassword))
        {
            throw new BadRequestException("Password and Confirm Password does not match.");
        }
        user.Password = passwordHasher.HashPassword(user, model.NewPassword);

        await _userRepository.UpdateAsync(user);
    }

    public async Task ResetPassword(int uid, string token, ResetPasswordRequest model)
    {
        var user = await _userRepository.FirstOrDefaultAsync(x => x.Id.Equals(uid) && x.PasswordResetToken!.Equals(token));
        if (user == null)
        {
            throw new BadRequestException("User is not found.");
        }
        if (user.ResetTokenExpires < DateTime.Now)
        {
            throw new BadRequestException("Token has been expired. Please initiate a new password reset request if you still need to reset your password.");
        }

        var passwordHasher = new PasswordHasher<User>();
        user.Password = passwordHasher.HashPassword(user, model.NewPassword);
        user.PasswordResetToken = null;
        user.ResetTokenExpires = null;
        await _userRepository.UpdateAsync(user);
    }


    public async Task<User> SignUpUser(SignUpUser model)
    {
        User entity = _mapper.Map(model, new User());
        var passwordHasher = new PasswordHasher<User>();
        entity.Password = passwordHasher.HashPassword(entity, model.Password);
        entity.IsActive = true;
        entity.Role = Role.Member;
        await _userRepository.CreateAsync(entity);
        return entity;
    }

    #region Generate JWT Token
    private string GenerateToken(User user)
    {
        var claims = new[] {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Username!),
                new Claim(ClaimTypes.Role, user.Role.ToString()!)
            };
        //Remember to change back to 2 hours
        //return new JwtSecurityTokenHandler().WriteToken(
        //    GenerateTokenByClaims(claims, DateTime.Now.AddMinutes(120))
        //    );
        return new JwtSecurityTokenHandler().WriteToken(
            GenerateTokenByClaims(claims, DateTime.Now.AddDays(1)));
    }

    private SecurityToken GenerateTokenByClaims(Claim[] claims, DateTime expires)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        return new JwtSecurityToken(_configuration["JWT:Issuer"],
             _configuration["JWT:Audience"],
             claims,
             expires: expires,
             signingCredentials: credentials);
    }
    #endregion

    private string CreateRandomToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    }
}