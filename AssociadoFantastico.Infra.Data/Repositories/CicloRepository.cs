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
    }
}
