using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Prueba_ABank.Data;
using Prueba_ABank.DTOs;
using Prueba_ABank.Models;

namespace Prueba_ABank.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher<Usuarios> _hasher;

    public UsersController(ApplicationDbContext db, IPasswordHasher<Usuarios> hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUser(int id)
    {
        var u = await _db.Usuarios.FindAsync(id);
        if (u == null) return NotFound();
        return Ok(new UserDto(u.Id, u.Nombres, u.Apellidos, u.FechaNacimiento, u.Direccion, u.Telefono, u.Email, u.Estado, u.FechaCreacion, u.FechaModificacion));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        if (await _db.Usuarios.AnyAsync(x => x.Email == dto.Email))
            return BadRequest(new { message = "Email ya registrado" });

        var user = new Usuarios
        {
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            FechaNacimiento = dto.FechaNacimiento,
            Direccion = dto.Direccion,
            Telefono = dto.Telefono,
            Email = dto.Email,
            FechaCreacion = DateTimeOffset.UtcNow,
            Estado = "A",
            Password = "" // se asigna luego
        };

        user.Password = _hasher.HashPassword(user, dto.Password);

        _db.Usuarios.Add(user);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new { id = user.Id });
    }

    /// <summary>
    /// Metodo para actualizar al usuario
    /// </summary>
    /// <param name="id">Id del usuario</param>
    /// <param name="dto">El modelo con los campos a modificar</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
    {
        var u = await _db.Usuarios.FindAsync(id);
        if (u == null) return NotFound();

        if (!string.IsNullOrEmpty(dto.Nombres)) u.Nombres = dto.Nombres;
        if (!string.IsNullOrEmpty(dto.Apellidos)) u.Apellidos = dto.Apellidos;
        if (dto.FechaNacimiento.HasValue) u.FechaNacimiento = dto.FechaNacimiento.Value;
        if (!string.IsNullOrEmpty(dto.Direccion)) u.Direccion = dto.Direccion;
        if (!string.IsNullOrEmpty(dto.Telefono)) u.Telefono = dto.Telefono;
        if (!string.IsNullOrEmpty(dto.Email)) u.Email = dto.Email;
        if (!string.IsNullOrEmpty(dto.Password)) u.Password = _hasher.HashPassword(u, dto.Password);

        u.FechaModificacion = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var u = await _db.Usuarios.FindAsync(id);
        if (u == null) return NotFound();

        u.Estado = "I";
        u.FechaModificacion = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync();
        return NoContent();
    }
}
