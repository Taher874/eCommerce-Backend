using eCommerce.DTOs.Login;
using eCommerce.DTOs.Register;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeonAuthApi.Data;
using NeonAuthApi.DTOs.Common;
using NeonAuthApi.Models;
using NeonAuthApi.Services;

namespace eCommerce.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly JwtService _jwtService;

    public AuthController(AppDbContext db, JwtService jwtService)
    {
        _db = db;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto dto)
    {
        if (await _db.Users.AnyAsync(x => x.Email == dto.Email))
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Email already exists",
                Data = null
            });
        }

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var response = new RegisterResponseDto
        {
            Token = _jwtService.GenerateToken(user),
            User = new RegisterUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            }
        };

        return Ok(new ApiResponse<RegisterResponseDto>
        {
            Success = true,
            Message = "Registered Successfully",
            Data = response
        });
    }


    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
        if (user == null)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Invalid email or password",
                Data = null
            });
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Invalid email or password",
                Data = null
            });
        }

        var response = new LoginResponseDto
        {
            Token = _jwtService.GenerateToken(user),
            User = new LoginUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            }
        };

        return Ok(new ApiResponse<LoginResponseDto>
        {
            Success = true,
            Message = "Logged in successfully",
            Data = response
        });
    }
}