using System;
using System.Collections.Generic;
using System.Linq;
using AssociadoFantastico.Application.Interfaces;
using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Domain.Exceptions;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace AssociadoFantastico.Application.Implementation
{
    public class CicloAppService : AppServiceBase<Ciclo, CicloViewModel>, ICicloAppService
    {
        public CicloAppService(IUnitOfWork unitOfWork, ICicloRepository repositoryBase, IMapper mapper) : base(unitOfWork, repositoryBase, mapper)
        {
        }

        public CicloViewModel Adicionar(NovoCicloViewModel novoCiclo)
        {
            var empresa = _unitOfWork.EmpresaRepository.BuscarPeloId(novoCiclo.EmpresaId);
            if (empresa == null) throw new NotFoundException("Empresa não encontrada.");

            var cicloExistente = (_repositoryBase as ICicloRepository)
                .BuscarPeloPeriodo(empresa.Id, novoCiclo.Ano, novoCiclo.Semestre);
            if (cicloExistente != null)
                throw new DuplicatedException("Já há um ciclo cadastrado para esse semestre.");

            var ciclo = new Ciclo(
                novoCiclo.Ano,
                novoCiclo.Semestre,
                novoCiclo.Descricao,
                _mapper.Map<Periodo>(novoCiclo.PeriodoVotacaoAssociadoFantastico),
                _mapper.Map<Periodo>(novoCiclo.PeriodoVotacaoAssociadoSuperFantastico),
                empresa);

            var cicloAnterior = (_repositoryBase as ICicloRepository)
                .BuscarCicloAnterior(empresa.Id, novoCiclo.Ano, novoCiclo.Semestre);

            if (cicloAnterior != null)
                foreach (var grupo in cicloAnterior.Grupos) ciclo.AdicionarGrupo(grupo);

            return base.Adicionar(ciclo);
        }

        public override void Atualizar(Guid id, CicloViewModel obj)
        {
            var ciclo = BuscarEntidade(id);
            ciclo.Descricao = obj.Descricao;
            base.Atualizar(ciclo);            
        }

        public IEnumerable<CicloViewModel> BuscarPelaEmpresa(Guid empresaId) =>
            _repositoryBase.BuscarTodos()
                .Where(c => c.EmpresaId == empresaId)
                .AsQueryable().ProjectTo<CicloViewModel>(_mapper.ConfigurationProvider);

        public VotacaoViewModel BuscarVotacaoPeloId(Guid cicloId, Guid votacaoId)
        {
            var ciclo = BuscarEntidade(cicloId);
            var votacao = ciclo.Votacoes.SingleOrDefault(v => v.Id == votacaoId);
            IsNotNull(votacao, "Votação", 'a');
            return _mapper.Map<VotacaoViewModel>(votacao);
        }

        public void IniciarVotacao(Guid cicloId, Guid votacaoId)
        {
            var ciclo = BuscarEntidade(cicloId);
            var votacao = ciclo.Votacoes.SingleOrDefault(v => v.Id == votacaoId);
            IsNotNull(votacao, "Votação", 'a');
            votacao.IniciarVotacao();
            base.Atualizar(ciclo);
        }

        public void FinalizarVotacao(Guid cicloId, Guid votacaoId)
        {
            var ciclo = BuscarEntidade(cicloId);
            var votacao = ciclo.Votacoes.SingleOrDefault(v => v.Id == votacaoId);
            IsNotNull(votacao, "Votação", 'a');
            votacao.FinalizarVotacao();
            base.Atualizar(ciclo);
        }

        public void AtualizarVotacao(Guid cicloId, Guid votacaoId, VotacaoViewModel votacao)
        {
            var ciclo = BuscarEntidade(cicloId);
            var votacaoEncontrada = ciclo.Votacoes.SingleOrDefault(v => v.Id == votacaoId);
            IsNotNull(votacaoEncontrada, "Votação", 'a');
            votacaoEncontrada.AtualizarPeriodoPrevisto(new Periodo(votacao.PeriodoPrevisto.DataInicio, votacao.PeriodoPrevisto.DataFim));
            base.Atualizar(ciclo);
        }

        public void AdicionarGrupo(Guid cicloId, GrupoViewModel grupo)
        {
            var ciclo = BuscarEntidade(cicloId);
            ciclo.AdicionarGrupo(_mapper.Map<Grupo>(grupo));
            base.Atualizar(ciclo);
        }

        public IEnumerable<GrupoViewModel> BuscarGrupos(Guid cicloId)
        {
            var ciclo = BuscarEntidade(cicloId);
            return ciclo.Grupos.AsQueryable().ProjectTo<GrupoViewModel>(_mapper.ConfigurationProvider);
        }

        public void AtualizarGrupo(Guid cicloId, GrupoViewModel grupo)
        {
            var ciclo = BuscarEntidade(cicloId);
            ciclo.AtualizarGrupo(_mapper.Map<Grupo>(grupo));
            base.Atualizar(ciclo);
        }

        public GrupoViewModel RemoverGrupo(Guid cicloId, Guid grupoId)
        {
            var ciclo = BuscarEntidade(cicloId);
            var grupo = _mapper.Map<GrupoViewModel>(ciclo.RemoverGrupo(new Grupo() { Id = grupoId }));
            base.Atualizar(ciclo);
            return grupo;
        }
    }
}
