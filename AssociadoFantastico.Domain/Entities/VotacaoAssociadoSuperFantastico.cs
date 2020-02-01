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

    }
}
