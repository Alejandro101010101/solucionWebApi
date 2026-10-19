using Microsoft.EntityFrameworkCore;
using WebAPISolution.Models;

namespace WebAPISolution.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Estudiante> Estudiante { get; set; }
        public DbSet<Curso> Curso { get; set; }
        public DbSet<Docente> Docente { get; set; }
        public DbSet<Asignacion> Asignacion { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relaciones 1 → N hacia Asignacion
            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.Estudiante)
                .WithMany(e => e.Asignaciones)
                .HasForeignKey(a => a.EstudianteId);

            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.Curso)
                .WithMany(c => c.Asignaciones)
                .HasForeignKey(a => a.CursoId);

            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.Docente)
                .WithMany(d => d.Asignaciones)
                .HasForeignKey(a => a.DocenteId);
        }
    }
}
