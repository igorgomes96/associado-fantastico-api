using AssociadoFantastico.Domain.Exceptions;
using System;

namespace AssociadoFantastico.Domain.Entities
{
    public class Voto : Entity
    {
        public Voto() { } // EF

        public Voto(Votacao votacao, Associado associado, Elegivel candidato, string ip): base()
        {
            Votacao = votacao ?? throw new CustomException("É preciso informar a qual votação se refere esse voto.");
            VotacaoId = votacao.Id;
            Eleitor = associado ?? throw new CustomException("É preciso informar o associado que está votando.");
            EleitorId = associado.Id;
            Candidato = candidato ?? throw new CustomException("É preciso informar o associado que está votando.");
            CandidatoId = candidato.Id;
            Horario = DateTime.Now;
            Ip = ip;
        }

        public Guid VotacaoId { get; private set; }
        public Guid EleitorId { get; private set; }
        public Guid CandidatoId { get; private set; }
        public DateTime Horario { get; private set; }
        public string Ip { get; private set; }

        public virtual Votacao Votacao { get; private set; }
        public virtual Associado Eleitor { get; private set; }
        public virtual Elegivel Candidato { get; private set; }
    }
}
