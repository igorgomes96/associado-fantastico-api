using AssociadoFantastico.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssociadoFantastico.Infra.Data.EntityConfig
{
    public class AssociadoConfiguration : IEntityTypeConfiguration<Associado>
    {

        public void Configure(EntityTypeBuilder<Associado> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(e => e.Aplausogramas)
                .IsRequired();

            builder.Property(u => u.Cargo)
                .HasMaxLength(255);

            builder.Property(u => u.Area)
                .HasMaxLength(255);

            builder.Property(u => u.CentroCusto)
                .HasMaxLength(100);

            builder.HasOne(a => a.Usuario)
                .WithMany()
                .HasForeignKey(a => a.UsuarioId)
                .IsRequired();

            builder.HasOne(a => a.Ciclo)
                .WithMany(c => c.Associados)
                .HasForeignKey(a => a.CicloId)
                .IsRequired();

            builder.HasOne(a => a.Grupo)
                .WithMany(g => g.Associados)
                .HasForeignKey(a => a.GrupoId)
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
