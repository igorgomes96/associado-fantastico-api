using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Infra.Data.EntityConfig;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AssociadoFantastico.Infra.Data.Context
{
    public class AssociadoFantasticoContext : DbContext
    {
        public AssociadoFantasticoContext(DbContextOptions<AssociadoFantasticoContext> options)
           : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmpresaConfiguration());
            modelBuilder.ApplyConfiguration(new GrupoConfiguration());
            modelBuilder.ApplyConfiguration(new CicloConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new AssociadoConfiguration());
            modelBuilder.ApplyConfiguration(new VotacaoConfiguration());
            modelBuilder.ApplyConfiguration(new ElegivelConfiguration());
            modelBuilder.ApplyConfiguration(new VotoConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Empresa> Empresas { get; set; }
        public virtual DbSet<Grupo> Grupos { get; set; }
        public virtual DbSet<Ciclo> Ciclos { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Associado> Associados { get; set; }
        public virtual DbSet<Elegivel> Elegiveis { get; set; }
        public virtual DbSet<Votacao> Votacoes { get; set; }
        public virtual DbSet<Voto> Votos { get; set; }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
            {
                entry.Property("Id").IsModified = false;
            }
            return base.SaveChanges();
        }

    }
}
