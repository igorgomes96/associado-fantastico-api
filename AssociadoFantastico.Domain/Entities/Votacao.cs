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

        public Elegivel AdicionarElegivel(Associado associado)
        {
            if (!Ciclo.Associados.Contains(associado))
                throw new CustomException("Associado não cadastrado nesse ciclo.");

            if (_elegiveis.Exists(e => e.Associado.Equals(associado)))
                throw new CustomException("Esse associado já está na lista de elegíveis para essa votação.");

            var elegivel = new Elegivel(associado, this);
            _elegiveis.Add(elegivel);
            return elegivel;
        }

        public virtual void IniciarVotacao()
        {
            if (PeriodoRealizado.DataInicio.HasValue)
                throw new CustomException("Essa votação já foi iniciada.");
            PeriodoRealizado.DataInicio = DateTime.Now;
        }

        public virtual void FinalizarVotacao()
        {
            ValidarPeriodoRealizadoParaVotacao();
            ApurarVotos();
            PeriodoRealizado.DataFim = DateTime.Now;
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
            _elegiveis.OrderByDescending(e => e.Pontuacao).ThenByDescending(e => e.Associado.Aplausogramas);

        public virtual void ApurarVotos()
        {
            ValidarPeriodoRealizadoParaApuracao();
            var grupos = Elegiveis.Select(e => e.Associado.Grupo).Distinct();
            foreach (var grupo in grupos)
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
