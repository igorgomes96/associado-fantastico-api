using AssociadoFantastico.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AssociadoFantastico.Domain.Entities
{
    public enum StatusImportacao
    {
        Aguardando,
        Processando,
        FinalizadoComSucesso,
        FinalizadoComFalha
    }

    public class Importacao : Entity
    {
        public Importacao() { }  // EF

        public Importacao(Votacao votacao, string arquivo, string cpfUsuarioImportacao)
        {
            Votacao = votacao ?? throw new CustomException("A votação precisa ser informada.");
            VotacaoId = votacao.Id;
            Status = StatusImportacao.Aguardando;
            CPFUsuarioImportacao = cpfUsuarioImportacao;
            Arquivo = arquivo;
        }

        public StatusImportacao Status { get; private set; }
        public Guid VotacaoId { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public string Arquivo { get; private set; }
        public string CPFUsuarioImportacao { get; private set; }

        public virtual Votacao Votacao { get; private set; }
        private readonly List<Inconsistencia> _inconsistencias = new List<Inconsistencia>();
        public virtual IReadOnlyCollection<Inconsistencia> Inconsistencias { get => new ReadOnlyCollection<Inconsistencia>(_inconsistencias); }

        public void IniciarProcessamento()
        {
            _inconsistencias.Clear();
            Status = StatusImportacao.Processando;
        }

        public void FinalizarProcessamentoComSucesso()
        {
            _inconsistencias.Clear();
            Status = StatusImportacao.FinalizadoComSucesso;
        }

        public void FinalizarImportacaoComFalha(IEnumerable<Inconsistencia> inconsistencias)
        {
            Status = StatusImportacao.FinalizadoComFalha;
            if (inconsistencias != null && inconsistencias.Any())
                _inconsistencias.AddRange(inconsistencias);
        }

    }
}
