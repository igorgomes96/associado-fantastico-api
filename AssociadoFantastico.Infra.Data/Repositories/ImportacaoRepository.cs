using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Infra.Data.Context;
using System.Linq;

namespace AssociadoFantastico.Infra.Data.Repositories
{
    public class ImportacaoRepository : RepositoryBase<Importacao>, IImportacaoRepository
    {
        public ImportacaoRepository(AssociadoFantasticoContext db) : base(db)
        {
        }

        public Importacao BuscarPrimeiraImportacaoPendenteDaFila() =>
            DbSet.Where(i => i.Status == StatusImportacao.Aguardando)
                .OrderBy(i => i.DataCadastro)
                .FirstOrDefault();
    }
}
