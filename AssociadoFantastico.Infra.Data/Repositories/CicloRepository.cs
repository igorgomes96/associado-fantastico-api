using System;
using System.Linq;
using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AssociadoFantastico.Infra.Data.Repositories
{
    public class CicloRepository : RepositoryBase<Ciclo>, ICicloRepository
    {
        public CicloRepository(AssociadoFantasticoContext db) : base(db)
        {
        }

        public Ciclo BuscarPeloPeriodo(Guid empresaId, int ano, int semestre) => BuscarTodos()
            .SingleOrDefault(c => c.Ano == ano && c.Semestre == semestre && c.EmpresaId == empresaId);

        public Ciclo BuscarCicloAnterior(Guid empresaId, int ano, int semestre) => BuscarTodos()
            .OrderByDescending(c => c.Ano).ThenByDescending(c => c.Semestre)
            .FirstOrDefault(c => c.Ano <= ano && c.Semestre <= semestre && c.EmpresaId == empresaId);

        public IQueryable<Associado> BuscarAssociados(Guid cicloId) =>
            _db.Set<Associado>().Include(a => a.Usuario).Where(a => a.CicloId == cicloId);

        public IQueryable<Associado> BuscarAssociados(Guid cicloId, Guid grupoId) =>
            BuscarAssociados(cicloId).Where(a => a.GrupoId == grupoId);

        public IQueryable<Elegivel> BuscarElegiveis(Guid cicloId, Guid votacaoId) =>
           _db.Set<Elegivel>()
            .Include(e => e.Associado)
                .ThenInclude(a => a.Usuario)
            .Where(e => e.Associado.CicloId == cicloId && e.VotacaoId == votacaoId);

        public IQueryable<Elegivel> BuscarElegiveis(Guid cicloId, Guid votacaoId, Guid grupoId) =>
            _db.Set<Elegivel>()
            .Include(e => e.Associado)
                .ThenInclude(a => a.Usuario)
            .Where(e => e.Associado.CicloId == cicloId && e.VotacaoId == votacaoId && e.Associado.GrupoId == grupoId);
    }
}
