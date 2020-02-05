using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Domain.Enums;
using AssociadoFantastico.Domain.Exceptions;
using AssociadoFantastico.Domain.Test.Fakes;
using AssociadoFantastico.Domain.Test.Helpers;
using System;
using System.Linq;
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
        public void AtualizarPeriodoPrevisto_VotacaoJaIniciadaDataInicioDiferente_ThrowsCustomException()
        {
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var votacao = new VotacaoFake(periodo, new Ciclo());
            votacao.IniciarVotacao();
            var novoPeriodo = new Periodo(new DateTime(2019, 12, 31), new DateTime(2020, 1, 2));
            var exception = Assert.Throws<CustomException>(() => votacao.AtualizarPeriodoPrevisto(novoPeriodo));
            Assert.Equal("Não é possível atualizar a data prevista para início após o início da votação.", exception.Message);
        }

        [Fact]
        public void AtualizarPeriodoPrevisto_VotacaoJaFinalizada_ThrowsCustomException()
        {
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var votacao = new VotacaoFake(periodo, new Ciclo());
            votacao.IniciarVotacao();
            votacao.FinalizarVotacao();
            var novoPeriodo = new Periodo(new DateTime(2019, 12, 31), new DateTime(2020, 1, 2));
            var exception = Assert.Throws<CustomException>(() => votacao.AtualizarPeriodoPrevisto(novoPeriodo));
            Assert.Equal("Não é possível atualizar a período previsto após o término da votação.", exception.Message);
        }

        [Fact]
        public void AtualizarPeriodoPrevisto_DatasNaoInformadas_ThrowsCustomException()
        {
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var votacao = new VotacaoFake(periodo, new Ciclo());
            votacao.IniciarVotacao();
            
            var novoPeriodo = new Periodo(new DateTime(2019, 12, 31), null);
            var exception = Assert.Throws<CustomException>(() => votacao.AtualizarPeriodoPrevisto(novoPeriodo));
            Assert.Equal("Para atualizar o período previsto é preciso informar a data de início de fim da votação.", exception.Message);
            
            novoPeriodo = new Periodo(null, new DateTime(2020, 1, 3));
            exception = Assert.Throws<CustomException>(() => votacao.AtualizarPeriodoPrevisto(novoPeriodo));
            Assert.Equal("Para atualizar o período previsto é preciso informar a data de início de fim da votação.", exception.Message);

            novoPeriodo = new Periodo(null, null);
            exception = Assert.Throws<CustomException>(() => votacao.AtualizarPeriodoPrevisto(novoPeriodo));
            Assert.Equal("Para atualizar o período previsto é preciso informar a data de início de fim da votação.", exception.Message);
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

        [Fact]
        public void AdicionarElegivel_AssociadoNaoCadastrado_ThrowsCustomException()
        {
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var votacao = new VotacaoFake(periodo, new Ciclo());

            var exception = Assert.Throws<CustomException>(() => votacao.AdicionarElegivel(new Associado()));
            Assert.Equal("Associado não cadastrado nesse ciclo.", exception.Message);

        }

        [Fact]
        public void AdicionarElegivel_AssociadoJaElegivel_ThrowsCustomException()
        {
            var ciclo = Factories.CriarCicloValido();
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var votacao = new VotacaoFake(periodo, ciclo);

            var grupo = new Grupo("Grupo 1");
            var usuario = new Usuario("12312312312", "111", "Usuário 1", "Cargo 1", "Área 1", ciclo.Empresa);
            var associado = new Associado(usuario, grupo, 10, "1234");
            ciclo.AdicionarAssociado(associado);
            votacao.AdicionarElegivel(associado);

            var exception = Assert.Throws<CustomException>(() => votacao.AdicionarElegivel(associado));
            Assert.Equal("Esse associado já está na lista de elegíveis para essa votação.", exception.Message);
        }

        [Fact]
        public void AdicionarElegivel_AssociadoNaoElegivel_ElegivelAdicionado()
        {
            var ciclo = Factories.CriarCicloValido();
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var votacao = new VotacaoFake(periodo, ciclo);

            var grupo = new Grupo("Grupo 1");
            var usuario = new Usuario("12312312312", "111", "Usuário 1", "Cargo 1", "Área 1", ciclo.Empresa);
            var associado = new Associado(usuario, grupo, 10, "1234");
            ciclo.AdicionarAssociado(associado);
            var elegivelRetornado = votacao.AdicionarElegivel(associado);

            Assert.Equal(associado, elegivelRetornado.Associado);
            Assert.Collection(votacao.Elegiveis, elegivel => Assert.Equal(elegivelRetornado, elegivel));
        }

        [Fact]
        public void ApurarVotos_VotacaoNaoIniciada_ThrowsCustomException()
        {
            var ciclo = Factories.CriarCicloValido();
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var votacao = new VotacaoFake(periodo, ciclo);

            var grupo = new Grupo("Grupo 1");
            var usuario = new Usuario("12312312312", "111", "Usuário 1", "Cargo 1", "Área 1", ciclo.Empresa);
            var associado = new Associado(usuario, grupo, 10, "1234");
            ciclo.AdicionarAssociado(associado);
            votacao.AdicionarElegivel(associado);
            var exception = Assert.Throws<CustomException>(() => votacao.ApurarVotos(grupo));

            Assert.Equal("Não é possível fazer a apuração dos votos antes do início da votação.", exception.Message);
        }

        [Fact]
        public void ApurarVotos_NenhumElegivelNoGrupo_RetornaListaVazia()
        {
            var ciclo = Factories.CriarCicloValido();
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var votacao = new VotacaoFake(periodo, ciclo);

            var grupo = new Grupo("Grupo 1");
            var usuario = new Usuario("12312312312", "111", "Usuário 1", "Cargo 1", "Área 1", ciclo.Empresa);
            var associado = new Associado(usuario, grupo, 10, "1234");
            ciclo.AdicionarAssociado(associado);
            var elegivelRetornado = votacao.AdicionarElegivel(associado);
            votacao.IniciarVotacao();
            elegivelRetornado.RegistrarVoto();

            Assert.Empty(votacao.ApurarVotos(new Grupo("Grupo 2")));
        }

        [Fact]
        public void ApurarVotos_GrupoValido_RetornaApuracao()
        {
            var ciclo = Factories.CriarCicloValido();
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var votacao = new VotacaoFake(periodo, ciclo, new Dimensionamento(2, 1));

            var grupo = new Grupo("Grupo 1");

            var usuario1 = new Usuario("12312312312", "111", "Usuário 1", "Cargo 1", "Área 1", ciclo.Empresa);
            var associado1 = new Associado(usuario1, grupo, 10, "1234");
            ciclo.AdicionarAssociado(associado1);

            var usuario2 = new Usuario("12312312311", "222", "Usuário 2", "Cargo 2", "Área 2", ciclo.Empresa);
            var associado2 = new Associado(usuario2, grupo, 11, "1234");
            ciclo.AdicionarAssociado(associado2);

            var elegivel1 = votacao.AdicionarElegivel(associado1);
            var elegivel2 = votacao.AdicionarElegivel(associado2);

            votacao.IniciarVotacao();
            elegivel1.RegistrarVoto();
            elegivel1.RegistrarVoto();
            elegivel2.RegistrarVoto();

            Assert.Collection(votacao.ApurarVotos(grupo),
                elegivel =>
                {
                    Assert.Equal(elegivel2, elegivel);
                    Assert.Equal(34, elegivel.Pontuacao);
                    Assert.Equal(EApuracao.Eleito, elegivel.Apuracao);
                },
                elegivel =>
                {
                    Assert.Equal(elegivel1, elegivel);
                    Assert.Equal(32, elegivel.Pontuacao);
                    Assert.Equal(EApuracao.NaoEleito, elegivel.Apuracao);
                });
        }

        [Fact]
        public void ApurarVotos_QtdaElegiveisMenorQueDimensionamento_RetornaApuracao()
        {
            var ciclo = Factories.CriarCicloValido();
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var votacao = new VotacaoFake(periodo, ciclo, new Dimensionamento(2, 1));

            var grupo = new Grupo("Grupo 1");

            votacao.IniciarVotacao();

            for (var i = 1; i <= 4; i++)
            {
                var usuario = new Usuario($"1231231231{i}", new string(i.ToString()[0], 4), $"Usuário {i}", $"Cargo {i}", $"Área {i}", ciclo.Empresa);
                var associado = new Associado(usuario, grupo, 10, "1234");
                ciclo.AdicionarAssociado(associado);
            }

            var elegivelRetornado = votacao.AdicionarElegivel(ciclo.Associados.ElementAt(2));

            Assert.Collection(votacao.ApurarVotos(grupo),
                elegivel =>
                {
                    Assert.Equal(elegivelRetornado, elegivel);
                    Assert.Equal(30, elegivel.Pontuacao);
                    Assert.Equal(EApuracao.Eleito, elegivel.Apuracao);
                });
        }

        [Fact]
        public void RetornarApuracao_VariosGrupos_RetornaApuracao()
        {
            var ciclo = Factories.CriarCicloValido();
            var periodo = new Periodo(new DateTime(2020, 1, 1), new DateTime(2020, 1, 2));
            var votacao = new VotacaoFake(periodo, ciclo, new Dimensionamento(2, 1));

            votacao.IniciarVotacao();

            var grupo1 = new Grupo("Grupo 1");
            // Aplausogramas: 3, 6, 9, 12
            for (var i = 1; i <= 4; i++)
            {
                var usuario = new Usuario($"1231231231{i}", new string(i.ToString()[0], 4), $"Usuário {i}", $"Cargo {i}", $"Área {i}", ciclo.Empresa);
                var associado = new Associado(usuario, grupo1, i, "1234");
                ciclo.AdicionarAssociado(associado);
                votacao.AdicionarElegivel(associado);
            }

            var grupo2 = new Grupo("Grupo 2");
            for (var i = 1; i <= 3; i++)
            {
                var usuario = new Usuario($"2231231231{i}", new string(i.ToString()[0], 4), $"Usuário {i}", $"Cargo {i}", $"Área {i}", ciclo.Empresa);
                var associado = new Associado(usuario, grupo2, 2, "1234");
                ciclo.AdicionarAssociado(associado);
                votacao.AdicionarElegivel(associado);
            }

            // Grupo 1
            // Pontuação: 7, 7, 9, 12
            votacao.Elegiveis.ElementAt(0).RegistrarVoto();
            votacao.Elegiveis.ElementAt(0).RegistrarVoto();
            votacao.Elegiveis.ElementAt(0).RegistrarVoto();
            votacao.Elegiveis.ElementAt(0).RegistrarVoto();
            votacao.Elegiveis.ElementAt(1).RegistrarVoto();

            // Grupo 2
            // Pontuação: 8, 6, 9
            votacao.Elegiveis.ElementAt(4).RegistrarVoto();
            votacao.Elegiveis.ElementAt(4).RegistrarVoto();
            votacao.Elegiveis.ElementAt(6).RegistrarVoto();
            votacao.Elegiveis.ElementAt(6).RegistrarVoto();
            votacao.Elegiveis.ElementAt(6).RegistrarVoto();

            votacao.FinalizarVotacao();

            Assert.Collection(votacao.RetornarApuracao(),
                grupo =>
                {
                    Assert.Equal(grupo1, grupo.Key);
                    Assert.Collection(grupo.Value,
                        elegivel =>
                        {
                            Assert.Equal(votacao.Elegiveis.ElementAt(3), elegivel);
                            Assert.Equal(12, elegivel.Pontuacao);
                        },
                        elegivel =>
                        {
                            Assert.Equal(votacao.Elegiveis.ElementAt(2), elegivel);
                            Assert.Equal(9, elegivel.Pontuacao);
                        },
                        elegivel =>
                        {
                            Assert.Equal(votacao.Elegiveis.ElementAt(1), elegivel);
                            Assert.Equal(7, elegivel.Pontuacao);
                        },
                        elegivel =>
                        {
                            Assert.Equal(votacao.Elegiveis.ElementAt(0), elegivel);
                            Assert.Equal(7, elegivel.Pontuacao);
                        });
                },
                grupo =>
                {
                    Assert.Equal(grupo2, grupo.Key);
                    Assert.Collection(grupo.Value,
                        elegivel =>
                        {
                            Assert.Equal(votacao.Elegiveis.ElementAt(6), elegivel);
                            Assert.Equal(9, elegivel.Pontuacao);
                        },
                        elegivel =>
                        {
                            Assert.Equal(votacao.Elegiveis.ElementAt(4), elegivel);
                            Assert.Equal(8, elegivel.Pontuacao);
                        },
                        elegivel =>
                        {
                            Assert.Equal(votacao.Elegiveis.ElementAt(5), elegivel);
                            Assert.Equal(6, elegivel.Pontuacao);
                        });
                });

        }
    }

}
