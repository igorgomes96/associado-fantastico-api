using AssociadoFantastico.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AssociadoFantastico.Domain.Entities
{
    public abstract class Votacao : Entity
    {
        protected Votacao() { }
        protected Votacao(Periodo periodoPrevisto, Ciclo ciclo): base()
        {
            PeriodoPrevisto = periodoPrevisto ?? 
                throw new CustomException("O período previsto para a realização da votação precisa ser informado");

            if (!periodoPrevisto.DataInicio.HasValue)
                throw new CustomException("A data de ínicio da votação deve ser informada.");

            if (!periodoPrevisto.DataFim.HasValue)
                throw new CustomException("A data de término da votação deve ser informada.");

            PeriodoRealizado = new Periodo(null, null);
            Ciclo = ciclo ?? throw new CustomException("O ciclo da votação precisa ser informado.");
            CicloId = ciclo.Id;
        }

        public virtual Periodo PeriodoPrevisto { get; private set; }
        public virtual Periodo PeriodoRealizado { get; private set; }
        public Guid CicloId { get; private set; }

        public virtual Ciclo Ciclo { get; private set; }
        private readonly List<Elegivel> _elegiveis = new List<Elegivel>();
        public virtual IReadOnlyCollection<Elegivel> Elegiveis => new ReadOnlyCollection<Elegivel>(_elegiveis);
        private readonly List<Voto> _votos = new List<Voto>();
        public virtual IReadOnlyCollection<Voto> Votos => new ReadOnlyCollection<Voto>(_votos);

        public virtual void IniciarVotacao()
        {
            if (PeriodoRealizado.DataInicio.HasValue)
                throw new CustomException("Essa votação já foi iniciada.");
            PeriodoRealizado.DataInicio = DateTime.Now;
        }

        public virtual void FinalizarVotacao()
        {
            if (!PeriodoRealizado.DataInicio.HasValue)
                throw new CustomException("Essa votação ainda não foi iniciada.");
            if (PeriodoRealizado.DataFim.HasValue)
                throw new CustomException("Essa votação já foi finalizada.");
            PeriodoRealizado.DataFim = DateTime.Now;
        }
    }
}
