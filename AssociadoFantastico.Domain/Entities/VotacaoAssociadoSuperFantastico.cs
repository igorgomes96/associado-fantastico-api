namespace AssociadoFantastico.Domain.Entities
{
    public class VotacaoAssociadoSuperFantastico : Votacao
    {
        public VotacaoAssociadoSuperFantastico() { }
        public VotacaoAssociadoSuperFantastico(Periodo periodoPrevisto, Ciclo ciclo) : base(periodoPrevisto, ciclo)
        {
        }

        public override void FinalizarVotacao()
        {
            throw new System.NotImplementedException();
        }
    }
}
