using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;

namespace AssociadoFantastico.Application.Interfaces
{
    public interface ICicloAppService: IAppServiceBase<Ciclo, CicloViewModel>
    {
        CicloViewModel Adicionar(NovoCicloViewModel novoCiclo);
    }
}
