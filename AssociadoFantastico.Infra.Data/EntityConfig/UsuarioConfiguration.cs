using AssociadoFantastico.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssociadoFantastico.Infra.Data.EntityConfig
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Cpf)
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(u => u.Matricula)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(u => u.Nome)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(u => u.Cargo)
                .HasMaxLength(255);

            builder.Property(u => u.Area)
                .HasMaxLength(255);

            builder.HasOne(u => u.Empresa)
                .WithMany()
                .HasForeignKey(u => u.EmpresaId)
                .IsRequired();

            builder.HasIndex(u => u.Cpf).IsUnique();
        }
    }
}
