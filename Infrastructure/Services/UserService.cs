using Application.Common.AuthResponses;
using Application.Common.Interfaces;
using Application.Dtos;
using Application.Entities.Token;
using Azure.Core;
using Domain.Entities.ApplicationUser;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public partial class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IOptions<JwtSettings> _JwtSettings;
    private readonly AppDbContext _context;

    public UserService(UserManager<ApplicationUser> userManager,
                       RoleManager<IdentityRole> roleManager,
                       IOptions<JwtSettings> JwtSettings, AppDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _JwtSettings = JwtSettings;
        _context = context;
    }


    public async Task<AuthResponse> RegisterAsync(RegisterRequestDto request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber

        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return new AuthResponse
            {
                IsSuccess = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }



        await AddUserToRoleAsync(user.Id, request.Role);


        // 🔥 هنا بقى الجديد
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var link = $"https://localhost:5001/api/auth/confirm-email?userId={user.Id}&token={token}";

        // مؤقتًا هنطبعه (بدل ما نبعت إيميل)
        Console.WriteLine(link);

        return new AuthResponse
        {
            IsSuccess = true,
            Message = "User registered. Check console for confirmation link"
        };
    
}


    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null) return false;

        var result = await _userManager.ConfirmEmailAsync(user, token);

        return result.Succeeded;
    }

    public async Task<AuthResponse> GeneratePasswordResetTokenAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)          return new AuthResponse { IsSuccess = false, };
        

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var link = $"https://localhost:5001/api/auth/reset-password?email={email}&token={token}";

        Console.WriteLine(link);

        return new AuthResponse { IsSuccess = true, };
        
    }

    public async Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return new AuthResponse
            {
                IsSuccess = false,
                Message = "Invalid Credential"
            };
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

        if (!result.Succeeded)
        {
            return new AuthResponse
            {
                IsSuccess = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }

        return new AuthResponse
        {
            IsSuccess = true,
            Message = "Password Changed Successfully"
        };
    }
    public async Task<AuthResponse> LoginAsync(LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return new AuthResponse
            {
                IsSuccess = false,
                Message = "Invalid email or password"
            };
        }
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid)
        {
            return new AuthResponse
            {
                IsSuccess = false,
                Message = "Invalid email or password"
            };
        }

        var Accesstoken = await GenerateJwtToken(user);


        var refreshToken = GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken), 
            Expires = DateTime.UtcNow.AddDays(7),
            UserId = user.Id,
            IsRevoked = false
        };

       
            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();
        

        return new AuthResponse
        {
            IsSuccess = true,
            AccessToken = Accesstoken,
            RefreshToken = refreshToken,
            Message = "Login successful"
        };
    }

    public async Task<bool> AddUserToRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null) return false;

        if (!role.ToLower().Equals("admin"))
            role = "User";

        if (!await _roleManager.RoleExistsAsync(role))
            await _roleManager.CreateAsync(new IdentityRole(role));

        await _userManager.AddToRoleAsync(user, role);

        return true;
    }

    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName!),
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_JwtSettings.Value.Key)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
             issuer: _JwtSettings.Value.Issuer,
            audience: _JwtSettings.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var user = await _context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)
        {
            return new AuthResponse
            {
                IsSuccess = false,
                Message = "Invalid email or password"
            };
        }

       if( string.IsNullOrEmpty(request.RefreshToken))
        {
            return new AuthResponse
            {
                IsSuccess = false,
                Message = "Invalid RefreshToken"
            };
        }


        var userRefreshTokens = user.RefreshTokens;

        var UserRefreshToken = userRefreshTokens
            .FirstOrDefault(r => BCrypt.Net.BCrypt.Verify(request.RefreshToken, r.RefreshTokenHash) &&  ! r.IsRevoked);

        if (UserRefreshToken == null)
        {
            return new AuthResponse
            {
                IsSuccess = false,
                Message = "Invalid RefreshToken"
            };
        }



        if (  UserRefreshToken.IsRevoked || UserRefreshToken.Expires <= DateTime.UtcNow)
        {
            
            return new AuthResponse
            {
                IsSuccess = false,
                Message = "Invalid RefreshToken"
            };
        }

 
        var Accesstoken = await GenerateJwtToken(user);

        var refreshToken = GenerateRefreshToken();

        UserRefreshToken.IsRevoked = true;

        _context.RefreshTokens.Add(new RefreshToken
        {
            RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
            UserId = user.Id,
            Expires = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
            
        });
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            IsSuccess = true,
            AccessToken = Accesstoken,
            RefreshToken = refreshToken,
            Message = "Token refreshed successfully"
        };

    }

    public async Task<AuthResponse> LogoutAsync(string email)
    {
        var user = await _context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == email);
          if (user == null)
            return new AuthResponse
            {
                IsSuccess = true,
                Message = "OK",
            };

        // Get all active refresh tokens
        var activeTokens = user.RefreshTokens
                               .Where(t => !t.IsRevoked && t.Expires > DateTime.UtcNow)
                               .ToList();

        foreach (var token in activeTokens)
        {
            token.IsRevoked = true;       // revoke the token
            token.Expires = DateTime.UtcNow; // optionally expire immediately
        }

        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            IsSuccess = true,
            Message = "OK"
        };
    }

}