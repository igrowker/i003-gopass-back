using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace template_csharp_dotnet.Models.Configurations
{
    public class UsuarioConfigurations : EntityTypeBaseConfiguration<Usuario>
    {
        protected override void ConfigurateConstraints(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.Id);
        }

        protected override void ConfigurateProperties(EntityTypeBuilder<Usuario> builder)
        {
            builder.Property(u => u.Nombre).IsRequired().HasMaxLength(200).HasColumnType("varchar");
            builder.Property(u => u.NumeroTelefono).IsRequired().HasMaxLength(10).HasColumnType("varchar");
            builder.Property(u => u.DNI).IsRequired().HasMaxLength(8).HasColumnType("varchar");
            builder.Property(u => u.Verificado).IsRequired().HasColumnType("bit");
        }

        protected override void ConfigurateTableName(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");
        }
    }
}
