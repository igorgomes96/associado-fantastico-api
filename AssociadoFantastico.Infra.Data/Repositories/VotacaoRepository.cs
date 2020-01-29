using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Infra.Data.Context;

namespace AssociadoFantastico.Infra.Data.Repositories
{
    public class VotacaoRepository : RepositoryBase<Votacao>, IVotacaoRepository
    {
        public VotacaoRepository(AssociadoFantasticoContext db) : base(db)
        {
        }
    }
}
