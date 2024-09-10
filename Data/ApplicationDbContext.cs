using Microsoft.EntityFrameworkCore;
using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Usuario> Usuarios{ get; set; }
        public DbSet<Reventa> Reventas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().Property(u => u.Nombre).IsRequired().HasMaxLength(100);

            base.OnModelCreating(modelBuilder);
        }
    }
}
