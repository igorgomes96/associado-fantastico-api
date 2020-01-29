using AssociadoFantastico.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssociadoFantastico.Infra.Data.EntityConfig
{
    public class VotoConfiguration : IEntityTypeConfiguration<Voto>
    {
        public void Configure(EntityTypeBuilder<Voto> builder)
        {
            builder.HasKey(v => v.Id);

            builder.HasOne(v => v.Votacao)
                .WithMany(v => v.Votos)
                .HasForeignKey(v => v.VotacaoId)
                .IsRequired()
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(v => v.Associado)
                .WithOne()
                .HasForeignKey<Voto>(v => v.AssociadoId)
                .IsRequired();

            builder.Property(v => v.Ip)
                .HasMaxLength(20);

            builder.Property(v => v.Horario)
                .IsRequired();
        }
    }
}
