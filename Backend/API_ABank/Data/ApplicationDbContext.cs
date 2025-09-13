using Microsoft.EntityFrameworkCore;
using Prueba_ABank.Models;

namespace Prueba_ABank.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts) : base(opts) { }

    public DbSet<Usuarios> Usuarios { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración para que FechaNacimiento sea "date" en PostgreSQL
        modelBuilder.Entity<Usuarios>()
            .Property(u => u.FechaNacimiento)
            .HasColumnType("date");

        base.OnModelCreating(modelBuilder);
    }
}
