using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Domain.Exceptions;
using System;
using Xunit;

namespace AssociadoFantastico.Domain.Test.Entities
{
    public class CicloTest
    {
        [Theory]
        [InlineData(2009, 1, null, null, null, null, "O ano deve ser maior que 2010.")]
        [InlineData(2010, 0, null, null, null, null, "O semestre deve ser 1 ou 2.")]
        [InlineData(2010, 3, null, null, null, null, "O semestre deve ser 1 ou 2.")]
        [InlineData(2010, 1, null, null, null, null, "O período previsto para a votação dos associados fantásticos deve ser informado.")]
        [InlineData(2010, 1, "2010/02/01", "2010/02/02", null, null, "O período previsto para a votação dos associados super fantásticos deve ser informado.")]
        [InlineData(2010, 1, "2010/02/01", "2010/02/02", "2010/01/01", "2010/01/02", "A data de início da votação do associado fantástico deve ser posterior à data de início da votação do associado super fantástico.")]
        public void NovoCiclo_PeriodoInvalido_ThrowsCustomException(
            int ano,
            int semestre,
            string inicioVotacao1,
            string fimVotacao1,
            string inicioVotacao2,
            string fimVotacao2,
            string mensagem)
        {
            var periodo1 = (inicioVotacao1 ?? fimVotacao1) == null ? null : 
                new Periodo(DateTime.Parse(inicioVotacao1), DateTime.Parse(fimVotacao1));
            var periodo2 = (inicioVotacao2 ?? fimVotacao2) == null ? null :
                new Periodo(DateTime.Parse(inicioVotacao2), DateTime.Parse(fimVotacao2));

            var empresa = new Empresa("Empresa teste");
            var exception = Assert.Throws<CustomException>(() => new Ciclo(ano, semestre, "Teste", periodo1, periodo2, empresa));
            Assert.Equal(mensagem, exception.Message);
        }

        [Fact]
        public void NovoCiclo_EmpresaNaoInformada_ThrowsCustomException()
        {
            var periodo = new Periodo(null, null);
            var exception = Assert.Throws<CustomException>(() => new Ciclo(2020, 1, "Teste", periodo, periodo, null));
            Assert.Equal("A empresa precisa ser informada para a criação de um novo ciclo.", exception.Message);
        }

        [Fact]
        public void NovoCiclo_ParametrosValidos_VotacoesCriadas()
        {
            var empresa = new Empresa("Empresa teste");
            var periodo1Inicio = new DateTime(2020, 1, 1);
            var periodo1Fim = new DateTime(2020, 1, 2);
            var periodo2Inicio = new DateTime(2020, 1, 3);
            var periodo2Fim = new DateTime(2020, 1, 4);
            var periodo1 = new Periodo(periodo1Inicio, periodo1Fim);
            var periodo2 = new Periodo(periodo2Inicio, periodo2Fim);
            var ciclo = new Ciclo(2020, 1, "Teste", periodo1, periodo2, empresa);

            Assert.Collection(ciclo.Votacoes,
                votacao =>
                {
                    Assert.IsType<VotacaoAssociadoFantastico>(votacao);
                    Assert.Equal(periodo1Inicio, votacao.PeriodoPrevisto.DataInicio);
                    Assert.Equal(periodo1Fim, votacao.PeriodoPrevisto.DataFim);
                },
                votacao =>
                {
                    Assert.IsType<VotacaoAssociadoSuperFantastico>(votacao);
                    Assert.Equal(periodo2Inicio, votacao.PeriodoPrevisto.DataInicio);
                    Assert.Equal(periodo2Fim, votacao.PeriodoPrevisto.DataFim);
                });
        }
    }
}
