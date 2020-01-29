using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Infra.Data.Context;

namespace AssociadoFantastico.Infra.Data.Repositories
{
    public class GrupoRepository : RepositoryBase<Grupo>, IGrupoRepository
    {
        public GrupoRepository(AssociadoFantasticoContext db) : base(db)
        {
        }
    }
}
