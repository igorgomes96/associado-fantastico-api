using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Infra.Data.Context;

namespace AssociadoFantastico.Infra.Data.Repositories
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AssociadoFantasticoContext db) : base(db)
        {
        }
    }
}
