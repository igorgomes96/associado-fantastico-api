using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssociadoFantastico.Application.Configurations;
using AssociadoFantastico.Application.Helpers;
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
        private const string PATH_FOTOS = @"StaticFiles\Fotos\";
        private const string PATH_IMPORTACOES = @"StaticFiles\Importacoes\";
        private readonly DimensionamentoPadraoAssociadoFantastico _dimensionamentoPadraoAssociadoFantastico;
        private readonly DimensionamentoPadraoAssociadoSuperFantastico _dimensionamentoPadraoAssociadoSuperFantastico;

        public CicloAppService(
            IUnitOfWork unitOfWork,
            ICicloRepository repositoryBase,
            IMapper mapper,
            DimensionamentoPadraoAssociadoFantastico dimensionamentoPadraoAssociadoFantastico,
            DimensionamentoPadraoAssociadoSuperFantastico dimensionamentoPadraoAssociadoSuperFantastico) : base(unitOfWork, repositoryBase, mapper, "Ciclo", 'o')
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

        private IQueryable<Elegivel> BuscarElegiveisEntidades(Guid cicloId, Guid votacaoId, Guid? grupoId = null) =>
            grupoId.HasValue ? (_repositoryBase as ICicloRepository).BuscarElegiveis(cicloId, votacaoId, grupoId.Value) :
                (_repositoryBase as ICicloRepository).BuscarElegiveis(cicloId, votacaoId);

        public IEnumerable<ElegivelViewModel> BuscarElegiveis(Guid cicloId, Guid votacaoId, Guid? grupoId = null) =>
            BuscarElegiveisEntidades(cicloId, votacaoId, grupoId).ProjectTo<ElegivelViewModel>(_mapper.ConfigurationProvider);

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

        public Stream BuscarFotoElegivel(Guid cicloId, Guid votacaoId, Guid elegivelId)
        {
            var ciclo = BuscarEntidade(cicloId);
            var votacao = ciclo.Votacoes.SingleOrDefault(v => v.Id == votacaoId);
            IsNotNull(votacao, "Votação", 'a');
            var elegivel = votacao.BuscarElegivelPeloId(elegivelId);
            IsNotNull(elegivel, "Associado", 'o');
            
            string file;
            if (string.IsNullOrEmpty(elegivel.Foto))
                file = FileSystemHelpers.GetAbsolutePath($@"{PATH_FOTOS}usuario.jpg");
            else
                file = FileSystemHelpers.GetAbsolutePath(elegivel.Foto);

            if (!File.Exists(file))
                throw new NotFoundException("Foto não encontrada.");

            return new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
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

        public IQueryable<Associado> BuscarAssociadosEntidades(Guid cicloId, Guid? grupoId = null) =>
            grupoId.HasValue ? (_repositoryBase as ICicloRepository).BuscarAssociados(cicloId, grupoId.Value)
                : (_repositoryBase as ICicloRepository).BuscarAssociados(cicloId);

        public IEnumerable<AssociadoViewModel> BuscarAssociados(Guid cicloId, Guid? grupoId = null) =>
            BuscarAssociadosEntidades(cicloId, grupoId).ProjectTo<AssociadoViewModel>(_mapper.ConfigurationProvider);

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

            var grupo = ciclo.BuscarGrupoPeloId(associado.GrupoId.Value);
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

            var grupo = ciclo.BuscarGrupoPeloId(associado.GrupoId.Value);
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

        public ElegivelViewModel AtualizarFotoElegivel(Guid cicloId, Guid votacaoId, Guid elegivelId, byte[] foto, string fotoFileName)
        {
            var ciclo = BuscarEntidade(cicloId);
            var votacao = ciclo.Votacoes.SingleOrDefault(v => v.Id == votacaoId);
            IsNotNull(votacao, "Votação", 'a');
            var elegivel = votacao.BuscarElegivelPeloId(elegivelId);
            IsNotNull(elegivel, "Associado", 'o');

            string relativePath = $@"{PATH_FOTOS}{votacaoId.ToString()}";
            string absolutePath = @FileSystemHelpers.GetAbsolutePath(relativePath);
            string tempPath = FileSystemHelpers.GetAbsolutePath($@"{relativePath}/temp");

            if (!Directory.Exists(absolutePath))
                Directory.CreateDirectory(absolutePath);

            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            // Salva a imagem original
            string originalFileName = @FileSystemHelpers.GetAbsolutePath(FileSystemHelpers.GetRelativeFileName(tempPath, fotoFileName));
            File.WriteAllBytes(originalFileName, foto);

            // Converte para JPEG, com 80% da qualidade
            string destinationFileName = FileSystemHelpers.GetRelativeFileName(relativePath, Path.ChangeExtension(fotoFileName, ".jpeg"));
            ImageHelpers.SalvarImagemJPEG(originalFileName, @FileSystemHelpers.GetAbsolutePath(destinationFileName), 80);

            // Exclui o arquivo orginal
            File.Delete(originalFileName);

            if (!string.IsNullOrWhiteSpace(elegivel.Foto))
            {
                var fotoAnterior = FileSystemHelpers.GetAbsolutePath(elegivel.Foto);
                if (File.Exists(fotoAnterior)) File.Delete(FileSystemHelpers.GetAbsolutePath(elegivel.Foto));
            }
            elegivel.Foto = destinationFileName;
            base.Atualizar(ciclo);
            return _mapper.Map<ElegivelViewModel>(elegivel);
        }

        public ImportacaoViewModel ImportarAssociados(Guid cicloId, Guid votacaoId, byte[] conteudoArquivo, string arquivo, string cpfUsuario)
        {
            var ciclo = BuscarEntidade(cicloId);
            var votacao = ciclo.Votacoes.SingleOrDefault(v => v.Id == votacaoId);
            IsNotNull(votacao, "Votação", 'a');

            string relativePath = $@"{PATH_IMPORTACOES}{votacaoId.ToString()}/Associados";
            string absolutePath = @FileSystemHelpers.GetAbsolutePath(relativePath);

            if (!Directory.Exists(absolutePath))
                Directory.CreateDirectory(absolutePath);

            string fullFileName = @FileSystemHelpers.GetAbsolutePath(FileSystemHelpers.GetRelativeFileName(absolutePath, arquivo));
            File.WriteAllBytes(fullFileName, conteudoArquivo);

            var importacao = new Importacao(votacao, Path.Combine(relativePath, arquivo), cpfUsuario);
            votacao.AdicionarImportacao(importacao);
            base.Atualizar(ciclo);
            return _mapper.Map<ImportacaoViewModel>(importacao);
        }

        public ImportacaoViewModel RetornarUltimaImportacao(Guid cicloId, Guid votacaoId)
        {
            var ciclo = BuscarEntidade(cicloId);
            var votacao = ciclo.Votacoes.SingleOrDefault(v => v.Id == votacaoId);
            IsNotNull(votacao, "Votação", 'a');

            var importacao = votacao.BuscarUltimaImportacao();
            return importacao == null ? null : _mapper.Map<ImportacaoViewModel>(importacao);
        }
    }
}
