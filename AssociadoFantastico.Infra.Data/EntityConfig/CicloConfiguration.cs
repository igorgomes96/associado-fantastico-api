using AssociadoFantastico.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssociadoFantastico.Infra.Data.EntityConfig
{
    public class CicloConfiguration : IEntityTypeConfiguration<Ciclo>
    {
        public void Configure(EntityTypeBuilder<Ciclo> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Ano).IsRequired();

            builder.Property(c => c.Semestre).IsRequired();

            builder.Property(c => c.DataInicio).IsRequired();

            builder.HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.EmpresaId)
                .IsRequired();

            builder.Property(c => c.Descricao)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(c => new { c.Ano, c.Semestre, c.EmpresaId }).IsUnique();
        }
    }
}
