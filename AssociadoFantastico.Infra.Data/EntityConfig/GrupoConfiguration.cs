using AssociadoFantastico.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssociadoFantastico.Infra.Data.EntityConfig
{
    public class GrupoConfiguration : IEntityTypeConfiguration<Grupo>
    {
        public void Configure(EntityTypeBuilder<Grupo> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Nome)
                .HasMaxLength(150)
                .IsRequired();

            builder.HasOne(g => g.Ciclo)
                .WithMany(c => c.Grupos)
                .HasForeignKey(g => g.CicloId)
                .IsRequired()
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(g => new { g.Nome, g.CicloId }).IsUnique();
        }
    }
}
