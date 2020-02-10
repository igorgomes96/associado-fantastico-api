using AssociadoFantastico.Domain.Entities;

namespace AssociadoFantastico.Application.Repositories
{
    public interface IImportacaoRepository: IRepositoryBase<Importacao>
    {
        Importacao BuscarPrimeiraImportacaoPendenteDaFila();
    }
}
