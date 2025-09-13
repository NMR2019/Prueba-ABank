namespace Prueba_ABank.DTOs;

public record CreateUserDto(string Nombres, string Apellidos, DateOnly FechaNacimiento, string Direccion, string Password, string Telefono, string Email);
public record UpdateUserDto(string? Nombres, string? Apellidos, DateOnly? FechaNacimiento, string? Direccion, string? Password, string? Telefono, string? Email);
public record LoginDto(string Email, string Password);
public record UserDto(int Id, string Nombres, string Apellidos, DateOnly FechaNacimiento, string Direccion, string Telefono, string Email, string Estado, DateTimeOffset FechaCreacion, DateTimeOffset? FechaModificacion);
