using AssociadoFantastico.Domain.Enums;
using AssociadoFantastico.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace AssociadoFantastico.Domain.Entities
{
    public class Elegivel: Entity
    {
        const int PESO_APLAUSOGRAMAS = 3;
        const int PESO_VOTOS = 1;

        public Elegivel() { } // EF

        public Elegivel(Associado associado, Votacao votacao): base()
        {
            Associado = associado ?? throw new CustomException("É obrigatório informar o associado para o cadastro de elegíveis.");
            AssociadoId = Associado.Id;
            Votacao = votacao ?? throw new CustomException("É obrigatório informar para qual votação o associado é elegível.");
            VotacaoId = votacao.Id;
            Apuracao = EApuracao.NaoApurado;
            Votos = 0;
        }

        
        public string Foto { get; set; }
        public Guid AssociadoId { get; private set; }
        public Guid VotacaoId { get; private set; }
        public EApuracao Apuracao { get; private set; }
        public int Votos { get; private set; }
        public int Pontuacao => (Associado.Aplausogramas * PESO_APLAUSOGRAMAS) + (Votos * PESO_VOTOS);

        public virtual Associado Associado { get; private set; }
        public virtual Votacao Votacao { get; private set; }

        public int RegistrarVoto() => ++Votos;

        public void RegistrarApuracao(EApuracao apuracao)
        {
            if (apuracao == EApuracao.NaoApurado)
                throw new CustomException("O registro da apuração deve ser 'Eleito' ou 'Não Eleito'.");
            Apuracao = apuracao;
        }

    }
}
