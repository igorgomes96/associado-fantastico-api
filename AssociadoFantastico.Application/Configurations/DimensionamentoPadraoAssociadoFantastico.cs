using AssociadoFantastico.Domain.Entities;
namespace AssociadoFantastico.Application.Configurations
{
    public class DimensionamentoPadraoAssociadoFantastico : Dimensionamento
    {
        public DimensionamentoPadraoAssociadoFantastico(int intervalo, int acrescimo): base(intervalo, acrescimo) { }
    }
}
