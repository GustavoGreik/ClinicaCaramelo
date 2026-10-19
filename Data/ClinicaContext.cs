using ClinicaCaramelo.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicaCaramelo.Data;

public class ClinicaContext : DbContext
{
    public ClinicaContext(DbContextOptions<ClinicaContext> options) : base(options) { }
    public DbSet<Tutor> Tutores => Set<Tutor>();
    public DbSet<Animal> Animais => Set<Animal>();
    public DbSet<Veterinario> Veterinarios => Set<Veterinario>();
    public DbSet<Consulta> Consultas => Set<Consulta>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tutor>().HasIndex(x => x.Cpf).IsUnique();
        modelBuilder.Entity<Tutor>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Veterinario>().HasIndex(x => x.Crmv).IsUnique();
        modelBuilder.Entity<Usuario>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Animal>().HasOne(x => x.Tutor).WithMany(x => x.Animais)
            .HasForeignKey(x => x.TutorId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Consulta>().HasOne(x => x.Animal).WithMany(x => x.Consultas)
            .HasForeignKey(x => x.AnimalId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Consulta>().HasOne(x => x.Veterinario).WithMany(x => x.Consultas)
            .HasForeignKey(x => x.VeterinarioId).OnDelete(DeleteBehavior.Restrict);
    }
}
