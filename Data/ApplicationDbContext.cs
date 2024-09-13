using Microsoft.EntityFrameworkCore;
using System.Reflection;
using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Reventa> Reventas { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
