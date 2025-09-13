using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Prueba_ABank.Data;
using Prueba_ABank.Models;
using Prueba_ABank.DTOs;
using Microsoft.Extensions.Options;


namespace Prueba_ABank.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher<Usuarios> _hasher;
    private readonly JwtOptions _jwt;

    public AuthController(ApplicationDbContext db, IPasswordHasher<Usuarios> hasher, IOptions<JwtOptions> jwtOptions)
    {
        _db = db;
        _hasher = hasher;
        _jwt = jwtOptions.Value;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email && u.Estado == "A");
        if (user == null) return Unauthorized(new { message = "Credenciales inválidas" });
        
        var result = _hasher.VerifyHashedPassword(user, user.Password, dto.Password);
        if (result == PasswordVerificationResult.Failed) return Unauthorized(new { message = "Credenciales inválidas" });

        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }

    private string GenerateJwtToken(Usuarios user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim("id", user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.ExpireMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
