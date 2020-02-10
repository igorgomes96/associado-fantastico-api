using AssociadoFantastico.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AssociadoFantastico.Domain.Entities
{
    public abstract class Votacao : Entity
    {
        protected Votacao() { }  // EF
        protected Votacao(Periodo periodoPrevisto, Ciclo ciclo, Dimensionamento dimensionamento) : base()
        {
            PeriodoPrevisto = periodoPrevisto ??
                throw new CustomException("O período previsto para a realização da votação precisa ser informado");

            if (!periodoPrevisto.DataInicio.HasValue)
                throw new CustomException("A data de ínicio da votação deve ser informada.");

            if (!periodoPrevisto.DataFim.HasValue)
                throw new CustomException("A data de término da votação deve ser informada.");

            PeriodoRealizado = new Periodo(null, null);
            Dimensionamento = dimensionamento ?? throw new CustomException("O dimensionamento da eleição precisa ser informado.");
            Ciclo = ciclo ?? throw new CustomException("O ciclo da votação precisa ser informado.");
            CicloId = ciclo.Id;
        }

        public virtual Periodo PeriodoPrevisto { get; private set; }
        public virtual Periodo PeriodoRealizado { get; private set; }
        public Guid CicloId { get; private set; }
        public virtual Dimensionamento Dimensionamento { get; private set; }

        public virtual Ciclo Ciclo { get; private set; }
        private readonly List<Elegivel> _elegiveis = new List<Elegivel>();
        public virtual IReadOnlyCollection<Elegivel> Elegiveis => new ReadOnlyCollection<Elegivel>(_elegiveis);
        private readonly List<Voto> _votos = new List<Voto>();
        public virtual IReadOnlyCollection<Voto> Votos => new ReadOnlyCollection<Voto>(_votos);
        private readonly List<Importacao> _importacoes = new List<Importacao>();
        public virtual IReadOnlyCollection<Importacao> Importacoes => new ReadOnlyCollection<Importacao>(_importacoes);

        public void AtualizarPeriodoPrevisto(Periodo periodo)
        {
            if (!periodo.DataInicio.HasValue || !periodo.DataFim.HasValue)
                throw new CustomException("Para atualizar o período previsto é preciso informar a data de início de fim da votação.");
            if (PeriodoRealizado.DataFim.HasValue)
                throw new CustomException("Não é possível atualizar a período previsto após o término da votação.");
            if (PeriodoRealizado.DataInicio.HasValue && PeriodoPrevisto.DataInicio != periodo.DataInicio)
                throw new CustomException("Não é possível atualizar a data prevista para início após o início da votação.");            

            PeriodoPrevisto = periodo;
        }

        public virtual Elegivel AdicionarElegivel(Associado associado)
        {
            if (PeriodoRealizado.DataFim.HasValue)
                throw new CustomException("Não é possível adicionar elegíveis após o término da votação.");

            if (!Ciclo.Associados.Contains(associado))
                throw new CustomException("Associado não cadastrado nesse ciclo.");

            if (Elegiveis.Any(e => e.Associado.Equals(associado)))
                throw new CustomException("Esse associado já está na lista de elegíveis para essa votação.");

            var elegivel = new Elegivel(associado, this);
            _elegiveis.Add(elegivel);
            return elegivel;
        }

        public virtual Elegivel RemoverElegivel(Elegivel elegivel)
        {
            if (PeriodoRealizado.DataFim.HasValue)
                throw new CustomException("Não é possível remover elegíveis após o término da votação.");

            if (!Elegiveis.Contains(elegivel))
                throw new CustomException("Elegível não cadastrado.");

            var elegivelRemovido = Elegiveis.Single(e => e.Equals(elegivel));
            _elegiveis.Remove(elegivelRemovido);
            return elegivelRemovido;
        }

        public Elegivel BuscarElegivelPeloId(Guid id) =>
            Elegiveis.FirstOrDefault(e => e.Id == id);

        public virtual void IniciarVotacao()
        {
            if (PeriodoRealizado.DataInicio.HasValue)
                throw new CustomException("Essa votação já foi iniciada.");
            PeriodoRealizado = new Periodo(DateTime.Now, null);
        }

        public virtual void FinalizarVotacao()
        {
            ValidarPeriodoRealizadoParaVotacao();
            ApurarVotos();
            PeriodoRealizado = new Periodo(PeriodoRealizado.DataInicio, DateTime.Now);
        }

        private void ValidarPeriodoRealizadoParaApuracao()
        {
            if (!PeriodoRealizado.DataInicio.HasValue)
                throw new CustomException("Não é possível fazer a apuração dos votos antes do início da votação.");
        }

        private void ValidarPeriodoRealizadoParaVotacao()
        {
            if (!PeriodoRealizado.DataInicio.HasValue)
                throw new CustomException("Essa votação ainda não foi iniciada.");
            if (PeriodoRealizado.DataFim.HasValue)
                throw new CustomException("Essa votação já foi finalizada.");
        }

        private IEnumerable<Elegivel> RetornarElegiveisOrdenadosPorPontuacao() =>
            Elegiveis.OrderByDescending(e => e.Pontuacao).ThenByDescending(e => e.Associado.Aplausogramas);

        public virtual void ApurarVotos()
        {
            ValidarPeriodoRealizadoParaApuracao();
            foreach (var grupo in Ciclo.Grupos)
                ApurarVotos(grupo);
        }

        public IEnumerable<Elegivel> ApurarVotos(Grupo grupo)
        {
            ValidarPeriodoRealizadoParaApuracao();
            var elegiveis = RetornarElegiveisOrdenadosPorPontuacao().Where(e => e.Associado.Grupo.Equals(grupo));
            if (!elegiveis.Any()) return new List<Elegivel>();

            var qtda = Dimensionamento.CalcularQuantidade(Ciclo.Associados.Count(a => a.Grupo.Equals(grupo)));

            foreach (var elegivel in elegiveis.Take(qtda))
                elegivel.RegistrarApuracao(Enums.EApuracao.Eleito);

            foreach (var elegivel in elegiveis.Skip(qtda))
                elegivel.RegistrarApuracao(Enums.EApuracao.NaoEleito);

            return elegiveis;
        }

        public void AdicionarImportacao(Importacao importacao)
        {
            this._importacoes.Add(importacao);
        }

        /// <summary>
        /// Retorna a apuração já registrada, após o término da votação.
        /// </summary>
        /// <param name="grupo"></param>
        /// <returns></returns>
        public IDictionary<Grupo, IEnumerable<Elegivel>> RetornarApuracao()
        {
            if (!PeriodoRealizado.DataFim.HasValue)
                throw new CustomException("Os votos serão apurados somente após o término da votação.");
            var elegiveis = RetornarElegiveisOrdenadosPorPontuacao();
            return elegiveis.GroupBy(e => e.Associado.Grupo)
                .ToDictionary(group => group.Key, group => group.ToList().AsEnumerable());
        }

        public Voto RegistrarVoto(Associado eleitor, Elegivel candidato, string ip)
        {
            ValidarPeriodoRealizadoParaVotacao();
            var voto = new Voto(this, eleitor, candidato, ip);
            _votos.Add(voto);
            candidato.RegistrarVoto();
            return voto;
        }

    }
}
