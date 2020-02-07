using System;
using System.Collections.Generic;
using System.Linq;
using AssociadoFantastico.Application.Configurations;
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
        private readonly DimensionamentoPadraoAssociadoFantastico _dimensionamentoPadraoAssociadoFantastico;
        private readonly DimensionamentoPadraoAssociadoSuperFantastico _dimensionamentoPadraoAssociadoSuperFantastico;

        public CicloAppService(
            IUnitOfWork unitOfWork,
            ICicloRepository repositoryBase,
            IMapper mapper,
            DimensionamentoPadraoAssociadoFantastico dimensionamentoPadraoAssociadoFantastico,
            DimensionamentoPadraoAssociadoSuperFantastico dimensionamentoPadraoAssociadoSuperFantastico) : base(unitOfWork, repositoryBase, mapper)
        {
            _dimensionamentoPadraoAssociadoSuperFantastico = dimensionamentoPadraoAssociadoSuperFantastico;
            _dimensionamentoPadraoAssociadoFantastico = dimensionamentoPadraoAssociadoFantastico;
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
                empresa,
                new Dimensionamento(_dimensionamentoPadraoAssociadoFantastico.Intervalo, _dimensionamentoPadraoAssociadoFantastico.Acrescimo),
                new Dimensionamento(_dimensionamentoPadraoAssociadoSuperFantastico.Intervalo, _dimensionamentoPadraoAssociadoSuperFantastico.Acrescimo));

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

        public IEnumerable<ElegivelViewModel> BuscarElegiveis(Guid cicloId, Guid votacaoId, Guid? grupoId = null)
        {
            var ciclo = BuscarEntidade(cicloId);
            var votacao = ciclo.Votacoes.SingleOrDefault(v => v.Id == votacaoId);
            IsNotNull(votacao, "Votação", 'a');
            var elegiveis = votacao.Elegiveis.AsQueryable();
            if (grupoId.HasValue)
                elegiveis = elegiveis.Where(e => e.Associado.GrupoId == grupoId.Value);
            return elegiveis.ProjectTo<ElegivelViewModel>(_mapper.ConfigurationProvider);
        }

        public ElegivelViewModel AdicionarElegivel(Guid cicloId, Guid votacaoId, Guid associadoId)
        {
            var ciclo = BuscarEntidade(cicloId);
            var votacao = ciclo.Votacoes.SingleOrDefault(v => v.Id == votacaoId);
            IsNotNull(votacao, "Votação", 'a');
            var associado = ciclo.BuscarAssociadoPeloId(associadoId);
            IsNotNull(associado, "Associado", 'o');
            var elegivel = _mapper.Map<ElegivelViewModel>(votacao.AdicionarElegivel(associado));
            base.Atualizar(ciclo);
            return elegivel;
        }

        public ElegivelViewModel RemoverElegivel(Guid cicloId, Guid votacaoId, Guid elegivelId)
        {
            var ciclo = BuscarEntidade(cicloId);
            var votacao = ciclo.Votacoes.SingleOrDefault(v => v.Id == votacaoId);
            IsNotNull(votacao, "Votação", 'a');
            var elegivel = votacao.BuscarElegivelPeloId(elegivelId);
            var elegivelRemovido = _mapper.Map<ElegivelViewModel>(votacao.RemoverElegivel(elegivel));
            base.Atualizar(ciclo);
            return elegivelRemovido;
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

        private IQueryable<Associado> BuscarAssociadosEntidades(Guid cicloId, Guid? grupoId = null)
        {
            var ciclo = BuscarEntidade(cicloId);
            var associados = ciclo.Associados.AsQueryable();
            if (grupoId.HasValue)
                associados = associados.Where(a => a.GrupoId == grupoId.Value);
            return associados;
        }

        public IEnumerable<AssociadoViewModel> BuscarAssociados(Guid cicloId, Guid? grupoId = null)
        {
            return BuscarAssociadosEntidades(cicloId, grupoId)
                .ProjectTo<AssociadoViewModel>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<AssociadoViewModel> BuscarAssociadosNaoElegiveis(Guid cicloId, Guid? grupoId = null)
        {
            return BuscarAssociadosEntidades(cicloId, grupoId)
                .Where(a => !a.Elegiveis.Any())
                .ProjectTo<AssociadoViewModel>(_mapper.ConfigurationProvider);
        }

        public void AdicionarAssociado(Guid cicloId, AssociadoViewModel associado)
        {
            var ciclo = BuscarEntidade(cicloId);
            var usuario = _unitOfWork.UsuarioRepository.BuscarPeloCPF(associado.Cpf);
            var empresa = _unitOfWork.EmpresaRepository.BuscarPeloId(associado.EmpresaId);
            IsNotNull(empresa, "Empresa", 'a');

            if (usuario == null)
                usuario = new Usuario(
                    associado.Cpf,
                    associado.Matricula,
                    associado.Nome,
                    associado.Cargo,
                    associado.Area,
                    empresa);

            var grupo = ciclo.BuscarGrupoPeloId(associado.GrupoId);
            IsNotNull(grupo, "Grupo", 'o');

            ciclo.AdicionarAssociado(new Associado(usuario, grupo, associado.Aplausogramas, associado.CentroCusto));
            base.Atualizar(ciclo);
        }

        public void AtualizarAssociado(Guid cicloId, AssociadoViewModel associado)
        {
            var ciclo = BuscarEntidade(cicloId);
            var usuario = _unitOfWork.UsuarioRepository.BuscarPeloId(associado.UsuarioId);
            IsNotNull(usuario, "Usuário", 'o');

            if (usuario.Cpf != associado.Cpf)
            {
                var usuarioCpf = _unitOfWork.UsuarioRepository.BuscarPeloCPF(associado.Cpf);
                if (usuarioCpf != null) throw new CustomException("Já existe um associado cadastrado com esse CPF!");
            }

            var associadoAtualizado = ciclo.BuscarAssociadoPeloId(associado.Id);
            IsNotNull(associadoAtualizado, "Associado", 'o');

            var grupo = ciclo.BuscarGrupoPeloId(associado.GrupoId);
            IsNotNull(grupo, "Grupo", 'o');

            var empresa = _unitOfWork.EmpresaRepository.BuscarPeloId(associado.EmpresaId);
            IsNotNull(empresa, "Empresa", 'a');

            var usuarioAtualizado = new Usuario(
                associado.Cpf,
                associado.Matricula,
                associado.Nome,
                associado.Cargo,
                associado.Area,
                empresa);

            usuario.AtualizarDados(usuarioAtualizado);
            associadoAtualizado.AtualizarDados(new Associado(
                usuarioAtualizado, grupo, associado.Aplausogramas, associado.CentroCusto));

            base.Atualizar(ciclo);
        }

        public void RemoverAssociado(Guid cicloId, Guid associadoId)
        {
            var ciclo = BuscarEntidade(cicloId);
            ciclo.RemoverAssociado(new Associado() { Id = associadoId });
            base.Atualizar(ciclo);
        }

        public AssociadoViewModel BuscarAssociado(Guid cicloId, Guid associadoId)
        {
            var ciclo = BuscarEntidade(cicloId);
            var associado = ciclo.BuscarAssociadoPeloId(associadoId);
            IsNotNull(associado, "Associado", 'o');
            return _mapper.Map<AssociadoViewModel>(associado);
        }

    }
}
