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

            builder.HasOne(a => a.Usuario)
                .WithMany()
                .HasForeignKey(a => a.UsuarioId)
                .IsRequired();

            builder.HasOne(a => a.Ciclo)
                .WithMany(c => c.Associados)
                .HasForeignKey(a => a.CicloId)
                .IsRequired()
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(a => a.Grupo)
                .WithMany(g => g.Associados)
                .HasForeignKey(a => a.GrupoId)
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
