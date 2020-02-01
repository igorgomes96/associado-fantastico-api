using AssociadoFantastico.Domain.Entities;
using Xunit;

namespace AssociadoFantastico.Domain.Test.Entities
{
    public class DimensionamentoTest
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(24, 0)]
        [InlineData(25, 2)]
        [InlineData(50, 2)]
        [InlineData(51, 2)]
        [InlineData(74, 2)]
        [InlineData(75, 4)]
        [InlineData(76, 4)]
        public void CalcularQuantidade(int qtda, int valor)
        {
            var dimensionamento = new Dimensionamento(50, 2);
            var qtdaRetornada = dimensionamento.CalcularQuantidade(qtda);
            Assert.Equal(valor, qtdaRetornada);
        }
    }
}
