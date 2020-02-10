using AssociadoFantastico.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AssociadoFantastico.Domain.Entities
{
    public class Ciclo: Entity
    {
        public Ciclo() { } // EF
        public Ciclo(
            int ano,
            int semestre,
            string descricao,
            Periodo periodoVotacaoAssociadoFantastico,
            Periodo periodoVotacaoAssociadoSuperFantastico,
            Empresa empresa,
            Dimensionamento dimensionamentoAssociadoFantastico = null,
            Dimensionamento dimensionamentoSuperAssociadoFantastico = null) : base()
        {
            if (ano < 2010) throw new CustomException("O ano deve ser maior que 2010.");
            if (semestre != 1 && semestre != 2) throw new CustomException("O semestre deve ser 1 ou 2.");

            Ano = ano;
            Semestre = semestre;
            Descricao = descricao;
            Empresa = empresa ?? throw new CustomException("A empresa precisa ser informada para a criação de um novo ciclo.");
            EmpresaId = empresa.Id;
            DataInicio = DateTime.Now;

            if (periodoVotacaoAssociadoFantastico == null)
                throw new CustomException("O período previsto para a votação dos associados fantásticos deve ser informado.");
            if (periodoVotacaoAssociadoSuperFantastico == null)
                throw new CustomException("O período previsto para a votação dos associados super fantásticos deve ser informado.");

            VotacaoAssociadoFantastico votacaoAssociadoFantastico;
            if (dimensionamentoAssociadoFantastico == null)
                votacaoAssociadoFantastico = new VotacaoAssociadoFantastico(periodoVotacaoAssociadoFantastico, this);
            else
                votacaoAssociadoFantastico = new VotacaoAssociadoFantastico(periodoVotacaoAssociadoFantastico, this, dimensionamentoAssociadoFantastico);

            VotacaoAssociadoSuperFantastico votacaoAssociadoSuperFantastico;
            if (dimensionamentoSuperAssociadoFantastico == null)
                votacaoAssociadoSuperFantastico = new VotacaoAssociadoSuperFantastico(periodoVotacaoAssociadoSuperFantastico, this);
            else
                votacaoAssociadoSuperFantastico = new VotacaoAssociadoSuperFantastico(periodoVotacaoAssociadoSuperFantastico, this, dimensionamentoSuperAssociadoFantastico);

            if (periodoVotacaoAssociadoFantastico.DataFim >= periodoVotacaoAssociadoSuperFantastico.DataInicio)
                throw new CustomException("A data de início da votação do associado fantástico deve ser posterior à data de início da votação do associado super fantástico.");

            _votacoes.Add(votacaoAssociadoFantastico);
            _votacoes.Add(votacaoAssociadoSuperFantastico);
        }

        public int Ano { get; private set; }
        public int Semestre { get; private set; }
        public string Descricao { get; set; }
        public Guid EmpresaId { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime? DataFinalizacao { get; private set; }

        public virtual Empresa Empresa { get; private set; }
        private readonly List<Associado> _associados = new List<Associado>();
        public virtual IReadOnlyCollection<Associado> Associados => new ReadOnlyCollection<Associado>(_associados);
        private readonly List<Votacao> _votacoes = new List<Votacao>();
        public virtual IReadOnlyCollection<Votacao> Votacoes => new ReadOnlyCollection<Votacao>(_votacoes);
        private readonly List<Grupo> _grupos = new List<Grupo>();
        public virtual IReadOnlyCollection<Grupo> Grupos => new ReadOnlyCollection<Grupo>(_grupos);


        private void ValidarNomeGrupo(Grupo grupo)
        {
            if (Grupos.Any(g => g.Nome == grupo.Nome))
                throw new DuplicatedException("Já há um grupo com esse nome cadastrado.");
        }

        public void AdicionarGrupo(Grupo grupo)
        {
            ValidarNomeGrupo(grupo);
            _grupos.Add(grupo);
        }

        public void AtualizarGrupo(Grupo grupo)
        {
            ValidarNomeGrupo(grupo);
            var grupoCadastado = Grupos.FirstOrDefault(g => g.Id == grupo.Id);
            if (grupoCadastado == null) throw new NotFoundException("Grupo não encontrado.");
            grupoCadastado.Nome = grupo.Nome;
        }

        public Grupo BuscarGrupoPeloId(Guid id) =>
            Grupos.SingleOrDefault(g => g.Id == id);

        public Grupo BuscarGrupoPeloNome(string nome) =>
            Grupos.SingleOrDefault(g => g.Nome.Trim().ToLower().Equals(nome.Trim().ToLower()));

        public Grupo RemoverGrupo(Grupo grupo)
        {
            var grupoRemovido = Grupos.FirstOrDefault(g => g.Equals(grupo));
            if (grupoRemovido == null) throw new NotFoundException("Grupo não encontrado.");
            if (grupoRemovido.Associados.Any())
                throw new CustomException("Não é possível excluir um grupo que tenha associados vinculados a ele.");
            
            _grupos.Remove(grupoRemovido);
            return grupoRemovido;
        }

        public void AdicionarAssociado(Associado associado)
        {
            if (Votacoes.First().PeriodoRealizado.DataFim.HasValue)
                throw new CustomException("Não é possível adicionar associados após o término da 1ª eleição.");

            if (Associados.Contains(associado) || Associados.Any(a => a.Usuario.Cpf == associado.Usuario.Cpf))
                throw new CustomException("Esse associado já foi cadastrado nesse ciclo.");

            if (!Grupos.Contains(associado.Grupo))
                throw new CustomException("Esse associado deve estar em um grupo habilitado para esse ciclo.");

            _associados.Add(associado);
        }

        public void AtualizarAssociado(Associado associadoAtualizado)
        {
            var associado = Associados.SingleOrDefault(a => a.Equals(associadoAtualizado));
            if (associado == null) throw new NotFoundException("Associado não cadastrado nesse ciclo.");
            if (!Grupos.Any(g => g.Equals(associadoAtualizado.Grupo)))
                throw new CustomException("Esse grupo não está habilitado para esse ciclo.");
            associado.AtualizarDados(associadoAtualizado);
        }

        public Associado RemoverAssociado(Associado associado)
        {
            if (Votacoes.First().PeriodoRealizado.DataFim.HasValue)
                throw new CustomException("Não é possível remover associados após o término da 1ª eleição.");

            if (!Associados.Contains(associado))
                throw new CustomException("Associado não cadastrado nesse ciclo.");

            var associadoRemovido = Associados.Single(a => a.Id == associado.Id);
            if (associadoRemovido.Elegiveis.Any())
                throw new CustomException("Não é possível remover um associado que seja elegível.");

            _associados.Remove(associado);
            return associadoRemovido;
        }


        public Associado BuscarAssociadoPeloId(Guid id) =>
            Associados.FirstOrDefault(a => a.Id == id);

        public Associado BuscarAssociadoPeloCPF(string cpf) =>
            Associados.FirstOrDefault(a => a.Usuario.Cpf == cpf);


        public void FinalizarCiclo()
        {
            if (Votacoes.Any(v => !v.PeriodoRealizado.DataFim.HasValue))
                throw new CustomException("Ainda existem votações em andamento nesse ciclo.");
            DataFinalizacao = DateTime.Now;
        }
    }
}
