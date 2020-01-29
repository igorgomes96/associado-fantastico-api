namespace AssociadoFantastico.Domain.Entities
{
    public class VotacaoAssociadoFantastico : Votacao
    {
        public VotacaoAssociadoFantastico() { }
        public VotacaoAssociadoFantastico(Periodo periodoPrevisto, Ciclo ciclo) : base(periodoPrevisto, ciclo)
        {
        }

        public override void FinalizarVotacao()
        {
            throw new System.NotImplementedException();
        }
    }
}
