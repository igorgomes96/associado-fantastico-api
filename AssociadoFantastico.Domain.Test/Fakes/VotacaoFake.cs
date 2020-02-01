using AssociadoFantastico.Domain.Entities;

namespace AssociadoFantastico.Domain.Test.Fakes
{
    public class VotacaoFake: Votacao
    {
        public VotacaoFake(Periodo periodoPrevisto, Ciclo ciclo) : base(periodoPrevisto, ciclo, new Dimensionamento(10, 2))
        {
        }

        public VotacaoFake(Periodo periodoPrevisto, Ciclo ciclo, Dimensionamento dimensionamento) : base(periodoPrevisto, ciclo, dimensionamento)
        {
        }
    }
}
