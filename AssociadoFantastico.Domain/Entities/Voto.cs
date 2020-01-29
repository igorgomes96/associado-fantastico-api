using AssociadoFantastico.Domain.Exceptions;
using System;

namespace AssociadoFantastico.Domain.Entities
{
    public class Voto : Entity
    {
        public Voto() { } // EF

        public Voto(Votacao votacao, Associado associado, string ip): base()
        {
            Votacao = votacao ?? throw new CustomException("É preciso informar a qual votação se refere esse voto.");
            VotacaoId = votacao.Id;
            Associado = associado ?? throw new CustomException("É preciso informar o associado que está votando.");
            AssociadoId = associado.Id;
            Horario = DateTime.Now;
            Ip = ip;
        }

        public Guid VotacaoId { get; private set; }
        public Guid AssociadoId { get; private set; }
        public DateTime Horario { get; private set; }
        public string Ip { get; private set; }

        public virtual Votacao Votacao { get; private set; }
        public virtual Associado Associado { get; private set; }
    }
}
