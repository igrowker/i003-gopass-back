using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace template_csharp_dotnet.Models.Configurations
{
    public class EntradaConfigurations : EntityTypeBaseConfiguration<Entrada>
    {
        protected override void ConfigurateConstraints(EntityTypeBuilder<Entrada> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(u => u.Usuario).WithOne(e => e.Entrada).HasForeignKey<Entrada>(u => u.UsuarioId);
        }

        protected override void ConfigurateProperties(EntityTypeBuilder<Entrada> builder)
        {
            builder.Property(e => e.CodigoQR).IsRequired().HasMaxLength(7089); //caracteres segun QR numerico a consultar
            builder.Property(e => e.Verificada).IsRequired();

        }

        protected override void ConfigurateTableName(EntityTypeBuilder<Entrada> builder)
        {
            builder.ToTable("Entradas");
        }
    }
}
