using System;
using System.Collections.Generic;
using System.Linq;
using AssociadoFantastico.Application.Exceptions;
using AssociadoFantastico.Application.Interfaces;
using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;
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

            if (_repositoryBase.BuscarTodos()
                .Any(c => c.Ano == novoCiclo.Ano && 
                    c.Semestre == novoCiclo.Semestre && c.EmpresaId == novoCiclo.EmpresaId))
                throw new DuplicatedException("Já há um ciclo cadastrado para esse semestre.");

            var ciclo = new Ciclo(
                novoCiclo.Ano,
                novoCiclo.Semestre,
                novoCiclo.Descricao,
                _mapper.Map<Periodo>(novoCiclo.PeriodoVotacaoAssociadoFantastico),
                _mapper.Map<Periodo>(novoCiclo.PeriodoVotacaoAssociadoSuperFantastico),
                empresa);

            return base.Adicionar(ciclo);
        }

        public override void Atualizar(Guid id, CicloViewModel obj)
        {
            var ciclo = _repositoryBase.BuscarPeloId(id);
            ciclo.Descricao = obj.Descricao;
            base.Atualizar(ciclo);            
        }

        public IEnumerable<CicloViewModel> BuscarPelaEmpresa(Guid empresaId) =>
            _repositoryBase.BuscarTodos()
                .Where(c => c.EmpresaId == empresaId)
                .AsQueryable().ProjectTo<CicloViewModel>(_mapper.ConfigurationProvider);

        public VotacaoViewModel BuscarVotacaoPeloId(Guid cicloId, Guid votacaoId)
        {
            var ciclo = _repositoryBase.BuscarPeloId(cicloId);
            var votacao = ciclo.Votacoes.SingleOrDefault(v => v.Id == votacaoId);
            IsNotNull(votacao, "Votação", 'a');
            return _mapper.Map<VotacaoViewModel>(votacao);
        }

        public void IniciarVotacao(Guid cicloId, Guid votacaoId)
        {
            var ciclo = _repositoryBase.BuscarPeloId(cicloId);
            var votacao = ciclo.Votacoes.SingleOrDefault(v => v.Id == votacaoId);
            IsNotNull(votacao, "Votação", 'a');
            votacao.IniciarVotacao();
            base.Atualizar(ciclo);
        }

        public void FinalizarVotacao(Guid cicloId, Guid votacaoId)
        {
            var ciclo = _repositoryBase.BuscarPeloId(cicloId);
            var votacao = ciclo.Votacoes.SingleOrDefault(v => v.Id == votacaoId);
            IsNotNull(votacao, "Votação", 'a');
            votacao.FinalizarVotacao();
            base.Atualizar(ciclo);
        }

        public void AtualizarVotacao(Guid cicloId, Guid votacaoId, VotacaoViewModel votacao)
        {
            var ciclo = _repositoryBase.BuscarPeloId(cicloId);
            var votacaoEncontrada = ciclo.Votacoes.SingleOrDefault(v => v.Id == votacaoId);
            IsNotNull(votacaoEncontrada, "Votação", 'a');
            votacaoEncontrada.AtualizarPeriodoPrevisto(new Periodo(votacao.PeriodoPrevisto.DataInicio, votacao.PeriodoPrevisto.DataFim));
            base.Atualizar(ciclo);
        }
    }
}
