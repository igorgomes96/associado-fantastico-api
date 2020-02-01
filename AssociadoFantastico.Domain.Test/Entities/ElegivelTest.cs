using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Domain.Enums;
using AssociadoFantastico.Domain.Exceptions;
using AssociadoFantastico.Domain.Test.Fakes;
using System;
using Xunit;

namespace AssociadoFantastico.Domain.Test.Entities
{
    public class ElegivelTest
    {
        [Fact]
        public void NovoElegivel_AssociadoNaoInformado_ThrowsException()
        {
            var exception = Assert.Throws<CustomException>(() => new Elegivel(null, null));
            Assert.Equal("É obrigatório informar o associado para o cadastro de elegíveis.", exception.Message);
        }

        [Fact]
        public void NovoElegivel_VotacaoNaoInformada_ThrowsException()
        {
            var exception = Assert.Throws<CustomException>(() => new Elegivel(new Associado(), null));
            Assert.Equal("É obrigatório informar para qual votação o associado é elegível.", exception.Message);
        }

        [Fact]
        public void RegistrarApuracao_ValorInvalido_ThrowsCustomException()
        {
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var votacao = new VotacaoFake(periodo, new Ciclo());
            var elegivel = new Elegivel(new Associado(), votacao);
            var exception = Assert.Throws<CustomException>(() => elegivel.RegistrarApuracao(EApuracao.NaoApurado));
            Assert.Equal("O registro da apuração deve ser 'Eleito' ou 'Não Eleito'.", exception.Message);
        }

        [Fact]
        public void RegistrarVoto_IncrementarVoto_RetornarVotos()
        {
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var votacao = new VotacaoFake(periodo, new Ciclo());
            var elegivel = new Elegivel(new Associado(), votacao);
            Assert.Equal(0, elegivel.Votos);
            var votos = elegivel.RegistrarVoto();
            Assert.Equal(1, votos);
            Assert.Equal(1, elegivel.Votos);
        }
    }
}
