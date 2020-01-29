using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Domain.Exceptions;
using AssociadoFantastico.Domain.Test.Fakes;
using System;
using Xunit;

namespace AssociadoFantastico.Domain.Test.Entities
{
    public class VotacaoTest
    {
        [Theory]
        [InlineData(null, "A data de ínicio da votação deve ser informada.")]
        [InlineData("2020/01/01", "A data de término da votação deve ser informada.")]
        public void NovaVotacao_DatasNaoInformadas_ThrowsCustomException(string dataInicio, string mensagem)
        {
            var periodo = new Periodo(dataInicio == null ? (DateTime?)null : DateTime.Parse(dataInicio), null);
            var exception = Assert.Throws<CustomException>(() => new VotacaoFake(periodo, new Ciclo()));
            Assert.Equal(mensagem, exception.Message);
        }

        [Fact]
        public void NovaVotacao_PeriodoPrevistoNaoInformado_ThrowsCustomException()
        {
            var exception = Assert.Throws<CustomException>(() => new VotacaoFake(null, new Ciclo()));
            Assert.Equal("O período previsto para a realização da votação precisa ser informado", exception.Message);
        }

        [Fact]
        public void NovaVotacao_CicloNaoInformado_ThrowsCustomException()
        {
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var exception = Assert.Throws<CustomException>(() => new VotacaoFake(periodo, null));
            Assert.Equal("O ciclo da votação precisa ser informado.", exception.Message);
        }

        [Fact]
        public void IniciarVotacao_VotacaoJaIniciada_ThrowsCustomException()
        {
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var votacao = new VotacaoFake(periodo, new Ciclo());
            votacao.IniciarVotacao();
            var exception = Assert.Throws<CustomException>(() => votacao.IniciarVotacao());
            Assert.Equal("Essa votação já foi iniciada.", exception.Message);
        }

        [Fact]
        public void IniciarVotacao_VotacaoNaoIniciada_DataInicioSetada()
        {
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var votacao = new VotacaoFake(periodo, new Ciclo());
            votacao.IniciarVotacao();
            Assert.NotNull(votacao.PeriodoRealizado.DataInicio);
        }

        [Fact]
        public void FinalizarVotacao_VotacaoJaFinalizada_ThrowsCustomException()
        {
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var votacao = new VotacaoFake(periodo, new Ciclo());
            votacao.IniciarVotacao();
            votacao.FinalizarVotacao();
            var exception = Assert.Throws<CustomException>(() => votacao.FinalizarVotacao());
            Assert.Equal("Essa votação já foi finalizada.", exception.Message);
        }


        [Fact]
        public void FinalizarVotacao_VotacaoNaoIniciada_ThrowsCustomException()
        {
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var votacao = new VotacaoFake(periodo, new Ciclo());
            var exception = Assert.Throws<CustomException>(() => votacao.FinalizarVotacao());
            Assert.Equal("Essa votação ainda não foi iniciada.", exception.Message);
        }

        [Fact]
        public void IniciarVotacao_VotacaoJaIniciadaNaoFinalizada_DataFimSetada()
        {
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var votacao = new VotacaoFake(periodo, new Ciclo());
            votacao.IniciarVotacao();
            votacao.FinalizarVotacao();
            Assert.NotNull(votacao.PeriodoRealizado.DataInicio);
            Assert.NotNull(votacao.PeriodoRealizado.DataFim);
        }
    }
}
