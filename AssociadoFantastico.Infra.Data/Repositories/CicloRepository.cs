using System;
using System.Linq;
using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Infra.Data.Context;

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
    }
}
