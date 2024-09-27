using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace template_csharp_dotnet.Models.Configurations
{
    public class UsuarioConfigurations : EntityTypeBaseConfiguration<Usuario>
    {
        protected override void ConfigurateConstraints(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.Id);
            builder.HasMany(e => e.Entrada).WithOne(r => r.Usuario).HasForeignKey(e => e.UsuarioId);
            builder.HasMany(e => e.Reventa).WithOne(r => r.Usuario).HasForeignKey(e => e.CompradorId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(e => e.Reventa).WithOne(r => r.Usuario).HasForeignKey(e => e.VendedorId).OnDelete(DeleteBehavior.Restrict);
        }

        protected override void ConfigurateProperties(EntityTypeBuilder<Usuario> builder)
        {
            builder.Property(u => u.Nombre).IsRequired().HasMaxLength(200).HasColumnType("varchar");
            builder.Property(u => u.NumeroTelefono).IsRequired().HasMaxLength(10).HasColumnType("varchar");
            builder.Property(u => u.DNI).IsRequired().HasMaxLength(8).HasColumnType("varchar");
            builder.Property(u => u.Verificado).IsRequired().HasColumnType("bit");
            builder.Property(u => u.Email).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Password).IsRequired();
            //builder.Property(u => u.Token);
        }

        protected override void ConfigurateTableName(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");
        }
    }
}
