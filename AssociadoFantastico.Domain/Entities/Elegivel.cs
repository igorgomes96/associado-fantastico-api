using AssociadoFantastico.Domain.Enums;
using AssociadoFantastico.Domain.Exceptions;
using System;

namespace AssociadoFantastico.Domain.Entities
{
    public class Elegivel: Entity
    {
        public Elegivel() { } // EF

        public Elegivel(int aplausogramas, Associado associado, Votacao votacao): base()
        {
            if (aplausogramas <= 0) throw new CustomException("A quantidade de aplausogramas deve ser maior que 0.");
            Aplausogramas = aplausogramas;
            Associado = associado ?? throw new CustomException("É obrigatório informar o associado para o cadastro de elegíveis.");
            AssociadoId = Associado.Id;
            Votacao = votacao ?? throw new CustomException("É obrigatório informar para qual votação o associado é elegível.");
            VotacaoId = votacao.Id;
            Apuracao = EApuracao.NaoApurado;
            Votos = 0;
        }

        public int Aplausogramas { get; private set; }
        public string Foto { get; private set; }
        public Guid AssociadoId { get; private set; }
        public Guid VotacaoId { get; private set; }
        public EApuracao Apuracao { get; private set; }
        public int Votos { get; private set; }

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
