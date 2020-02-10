using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;

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
        void AdicionarGrupo(Guid cicloId, GrupoViewModel grupo);
        void AtualizarGrupo(Guid cicloId, GrupoViewModel grupo);
        IEnumerable<GrupoViewModel> BuscarGrupos(Guid cicloId);
        GrupoViewModel RemoverGrupo(Guid cicloId, Guid grupoId);
        IEnumerable<AssociadoViewModel> BuscarAssociados(Guid cicloId, Guid? grupoId = null);
        void AdicionarAssociado(Guid cicloId, AssociadoViewModel associado);
        void AtualizarAssociado(Guid cicloId, AssociadoViewModel associado);
        void RemoverAssociado(Guid cicloId, Guid associadoId);
        AssociadoViewModel BuscarAssociado(Guid cicloId, Guid associadoId);
        ElegivelViewModel AdicionarElegivel(Guid cicloId, Guid votacaoId, Guid associadoId);
        IEnumerable<ElegivelViewModel> BuscarElegiveis(Guid cicloId, Guid votacaoId, Guid? grupoId = null);
        IEnumerable<AssociadoViewModel> BuscarAssociadosNaoElegiveis(Guid cicloId, Guid? grupoId = null);
        ElegivelViewModel RemoverElegivel(Guid cicloId, Guid votacaoId, Guid elegivelId);
        ElegivelViewModel AtualizarFotoElegivel(Guid cicloId, Guid votacaoId, Guid elegivelId, byte[] foto, string fotoFileName);
        Stream BuscarFotoElegivel(Guid cicloId, Guid votacaoId, Guid elegivelId);
        ImportacaoViewModel ImportarAssociados(Guid cicloId, Guid votacaoId, byte[] conteudoArquivo, string arquivo, string cpfUsuario);

    }
}
