using System.Collections.Generic;
using System.Linq;

namespace AssociadoFantastico.Domain.Entities
{
    public class VotacaoAssociadoFantastico : Votacao
    {
        public VotacaoAssociadoFantastico() { }
        public VotacaoAssociadoFantastico(
            Periodo periodoPrevisto,
            Ciclo ciclo) : base(periodoPrevisto, ciclo, new Dimensionamento(50, 2))
        {
        }

        public VotacaoAssociadoFantastico(
            Periodo periodoPrevisto,
            Ciclo ciclo,
            Dimensionamento dimensionamento) : base(periodoPrevisto, ciclo, dimensionamento)
        {
        }
    }
}
