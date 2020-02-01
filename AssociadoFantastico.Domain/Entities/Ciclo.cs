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
            Empresa empresa) : base()
        {
            if (ano < 2010) throw new CustomException("O ano deve ser maior que 2010.");
            if (semestre != 1 && semestre != 2) throw new CustomException("O semestre deve ser 1 ou 2.");

            Ano = ano;
            Semestre = semestre;
            Descricao = descricao;
            Empresa = empresa ?? throw new CustomException("A empresa precisa ser informada para a criação de um novo ciclo.");
            EmpresaId = empresa.Id;

            var votacaoAssociadoFantastico = new VotacaoAssociadoFantastico(
                periodoVotacaoAssociadoFantastico ?? throw new CustomException("O período previsto para a votação dos associados fantásticos deve ser informado."), this);
            var votacaoAssociadoSuperFantastico = new VotacaoAssociadoSuperFantastico(
                periodoVotacaoAssociadoSuperFantastico ?? throw new CustomException("O período previsto para a votação dos associados super fantásticos deve ser informado."), this);

            if (periodoVotacaoAssociadoFantastico.DataFim >= periodoVotacaoAssociadoSuperFantastico.DataInicio)
                throw new CustomException("A data de início da votação do associado fantástico deve ser posterior à data de início da votação do associado super fantástico.");

            _votacoes.Add(votacaoAssociadoFantastico);
            _votacoes.Add(votacaoAssociadoSuperFantastico);
        }

        public int Ano { get; private set; }
        public int Semestre { get; private set; }
        public string Descricao { get; private set; }
        public Guid EmpresaId { get; private set; }

        public virtual Empresa Empresa { get; private set; }
        private readonly List<Associado> _associados = new List<Associado>();
        public virtual IReadOnlyCollection<Associado> Associados => new ReadOnlyCollection<Associado>(_associados);
        private readonly List<Votacao> _votacoes = new List<Votacao>();
        public virtual IReadOnlyCollection<Votacao> Votacoes => new ReadOnlyCollection<Votacao>(_votacoes);

        public void AdicionarAssociado(Associado associado)
        {
            if (_votacoes.First().PeriodoRealizado.DataFim.HasValue)
                throw new CustomException("Não é possível adicionar associados após o término da 1ª eleição.");

            if (_associados.Contains(associado) || _associados.Exists(a => a.Usuario.Cpf == associado.Usuario.Cpf))
                throw new CustomException("Esse associado já foi cadastrado nesse ciclo.");

            _associados.Add(associado);
        }

        public Associado RemoverAssociado(Associado associado)
        {
            if (_votacoes.First().PeriodoRealizado.DataFim.HasValue)
                throw new CustomException("Não é possível remover associados após o término da 1ª eleição.");

            if (!_associados.Contains(associado))
                throw new CustomException("Associado não cadastrado nesse ciclo.");

            var associadoRemovido = _associados.Find(a => a.Id == associado.Id);
            _associados.Remove(associado);
            return associadoRemovido;

        }
    }
}
