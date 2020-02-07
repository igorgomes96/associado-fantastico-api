using AssociadoFantastico.Domain.Entities;

namespace AssociadoFantastico.Application.Repositories
{
    public interface IUsuarioRepository: IRepositoryBase<Usuario>
    {
        Usuario BuscarPeloCPF(string cpf);
    }
}
