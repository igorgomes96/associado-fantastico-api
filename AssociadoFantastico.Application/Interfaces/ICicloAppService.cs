using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;
using System;
using System.Collections.Generic;

namespace AssociadoFantastico.Application.Interfaces
{
    public interface ICicloAppService: IAppServiceBase<Ciclo, CicloViewModel>
    {
        CicloViewModel Adicionar(NovoCicloViewModel novoCiclo);
        IEnumerable<CicloViewModel> BuscarPelaEmpresa(Guid empresaId);
        VotacaoViewModel BuscarVotacaoPeloId(Guid cicloId, Guid votacaoId);
        void IniciarVotacao(Guid cicloId, Guid votacaoId);
        void FinalizarVotacao(Guid cicloId, Guid votacaoId);
        void AtualizarVotacao(Guid cicloId, Guid votacaoId, VotacaoViewModel votacao);

    }
}
