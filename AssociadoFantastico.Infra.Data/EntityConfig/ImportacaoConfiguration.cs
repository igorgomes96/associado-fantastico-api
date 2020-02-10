using AssociadoFantastico.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssociadoFantastico.Infra.Data.EntityConfig
{
    public class ImportacaoConfiguration : IEntityTypeConfiguration<Importacao>
    {
        public void Configure(EntityTypeBuilder<Importacao> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Status)
                .IsRequired();

            builder.Property(i => i.Arquivo)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(i => i.CPFUsuarioImportacao)
               .HasMaxLength(11)
               .IsRequired();

            builder.HasOne(i => i.Votacao)
                .WithMany(e => e.Importacoes)
                .HasForeignKey(i => i.VotacaoId)
                .IsRequired();

            builder.Property(i => i.DataCadastro)
                .IsRequired();
        }
    }
}
