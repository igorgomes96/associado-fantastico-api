using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Infra.Data.Context;
using System.Linq;

namespace AssociadoFantastico.Infra.Data.Repositories
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AssociadoFantasticoContext db) : base(db)
        {
        }

        public Usuario BuscarPeloCPF(string cpf) =>
            BuscarTodos().SingleOrDefault(u => u.Cpf == cpf);
    }
}
