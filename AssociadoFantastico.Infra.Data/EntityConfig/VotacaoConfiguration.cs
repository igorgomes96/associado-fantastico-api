using AssociadoFantastico.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssociadoFantastico.Infra.Data.EntityConfig
{
    public class VotacaoConfiguration : IEntityTypeConfiguration<Votacao>
    {
        public void Configure(EntityTypeBuilder<Votacao> builder)
        {
            builder.HasKey(v => v.Id);

            builder.OwnsOne(v => v.PeriodoPrevisto)
                .Property(v => v.DataInicio)
                .IsRequired();

            builder.OwnsOne(v => v.PeriodoPrevisto)
                .Property(v => v.DataFim)
                .IsRequired();

            builder.OwnsOne(v => v.PeriodoRealizado);

            builder.HasOne(v => v.Ciclo)
                .WithMany(c => c.Votacoes)
                .HasForeignKey(v => v.CicloId)
                .IsRequired()
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasDiscriminator<string>("TipoVotacao")
                .HasValue<VotacaoAssociadoFantastico>("Associado Fantástico")
                .HasValue<VotacaoAssociadoSuperFantastico>("Associado Super Fantástico");


        }
    }
}
