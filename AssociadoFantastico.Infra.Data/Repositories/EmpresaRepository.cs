using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Infra.Data.Context;

namespace AssociadoFantastico.Infra.Data.Repositories
{
    public class EmpresaRepository : RepositoryBase<Empresa>, IEmpresaRepository
    {
        public EmpresaRepository(AssociadoFantasticoContext db) : base(db)
        {
        }
    }
}
