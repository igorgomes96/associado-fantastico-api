using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;

namespace AssociadoFantastico.Application.Interfaces
{
    public interface IUsuarioAppService: IAppServiceBase<Usuario, UsuarioViewModel>
    {
        UsuarioViewModel BuscarUsuario(string cpf, string matricula);
    }
}
