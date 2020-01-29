using AssociadoFantastico.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssociadoFantastico.Infra.Data.EntityConfig
{
    public class ElegivelConfiguration : IEntityTypeConfiguration<Elegivel>
    {
        public void Configure(EntityTypeBuilder<Elegivel> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Aplausogramas)
                .IsRequired();

            builder.Property(e => e.Foto)
                .HasMaxLength(255);

            builder.HasOne(e => e.Associado)
                .WithMany()
                .HasForeignKey(e => e.AssociadoId)
                .IsRequired();

            builder.HasOne(e => e.Votacao)
                .WithMany(v => v.Elegiveis)
                .HasForeignKey(e => e.VotacaoId)
                .IsRequired()
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(e => new { e.VotacaoId, e.AssociadoId }).IsUnique();
        }
    }
}
