using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prueba_ABank.Models;

[Table("usuarios")]
public class Usuarios
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required, Column("nombres"), MaxLength(100)]
    public string Nombres { get; set; } = null!;

    [Required, Column("apellidos"), MaxLength(100)]
    public string Apellidos { get; set; } = null!;

    [Required, Column("fecha_nacimiento")]
    public DateOnly FechaNacimiento { get; set; }

    [Required, Column("direccion")]
    public string Direccion { get; set; } = null!;

    [Required, Column("password"), MaxLength(120)]
    public string Password { get; set; } = null!;

    [Required, Column("telefono"), MaxLength(20)]
    public string Telefono { get; set; } = null!;

    [Required, Column("email"), MaxLength(255)]
    public string Email { get; set; } = null!;

    [Required, Column("estado"), MaxLength(1)]
    public string Estado { get; set; } = "A";

    [Required, Column("fecha_creacion")]
    public DateTimeOffset FechaCreacion { get; set; }

    [Column("fecha_modificacion")]
    public DateTimeOffset? FechaModificacion { get; set; }
}

