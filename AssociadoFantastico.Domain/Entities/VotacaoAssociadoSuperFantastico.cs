using AssociadoFantastico.Domain.Exceptions;

namespace AssociadoFantastico.Domain.Entities
{
    public class VotacaoAssociadoSuperFantastico : Votacao
    {
        public VotacaoAssociadoSuperFantastico() { }
        public VotacaoAssociadoSuperFantastico(
             Periodo periodoPrevisto,
             Ciclo ciclo) : base(periodoPrevisto, ciclo, new Dimensionamento(0, 2))
        {
        }

        public VotacaoAssociadoSuperFantastico(
            Periodo periodoPrevisto,
            Ciclo ciclo,
            Dimensionamento dimensionamento) : base(periodoPrevisto, ciclo, dimensionamento)
        {
        }

        public override void FinalizarVotacao()
        {
            base.FinalizarVotacao();
            Ciclo.FinalizarCiclo();
        }

        public override Elegivel AdicionarElegivel(Associado associado)
        {
            if (PeriodoRealizado.DataInicio.HasValue)
                throw new CustomException("Os elegíveis da votação de associados super fantásticos foram definidos na apuração da votação dos associados fantásticos.");
            return base.AdicionarElegivel(associado);
        }

    }
}
