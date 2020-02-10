using AssociadoFantastico.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssociadoFantastico.Infra.Data.EntityConfig
{
    public class InconsistenciaConfiguration : IEntityTypeConfiguration<Inconsistencia>
    {
        public void Configure(EntityTypeBuilder<Inconsistencia> builder)
        {
            builder.HasKey(i => i.Id);

            builder.HasOne(i => i.Importacao)
                .WithMany(i => i.Inconsistencias)
                .HasForeignKey(i => i.ImportacaoId)
                .IsRequired();

            builder.Property(i => i.Coluna)
                .HasMaxLength(100);

            builder.Property(i => i.Mensagem)
                .HasMaxLength(255);
        }
    }
}
