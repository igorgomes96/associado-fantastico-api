using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Domain.Exceptions;
using AssociadoFantastico.Domain.Test.Helpers;
using System;
using System.Linq;
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

        [Fact]
        public void AdicionarGrupo_GrupoValido_GrupoAdicionado()
        {
            var ciclo = Factories.CriarCicloValido();
            ciclo.AdicionarGrupo(new Grupo("Grupo 1"));
            Assert.Equal(1, ciclo.Grupos.Count);
        }

        [Fact]
        public void AdicionarGrupo_GrupoComNomeDuplicado_ThrowsDuplicatedException()
        {
            var ciclo = Factories.CriarCicloValido();
            ciclo.AdicionarGrupo(new Grupo("Grupo 1"));
            var exception = Assert.Throws<DuplicatedException>(() => ciclo.AdicionarGrupo(new Grupo("Grupo 1")));
            Assert.Equal("Já há um grupo com esse nome cadastrado.", exception.Message);
        }

        [Fact]
        public void AtualizarGrupo_GrupoNaoEncontrado_ThrowsNotFoundException()
        {
            var ciclo = Factories.CriarCicloValido();
            var exception = Assert.Throws<NotFoundException>(() => ciclo.AtualizarGrupo(new Grupo("Teste")));
            Assert.Equal("Grupo não encontrado.", exception.Message);
        }

        [Fact]
        public void AtualizarGrupo_GrupoEncontrado_AtualizaNome()
        {
            var ciclo = Factories.CriarCicloValido();
            var grupoAdicionado = new Grupo("Grupo 1");
            ciclo.AdicionarGrupo(grupoAdicionado);
            var grupoAtualizado = new Grupo("Teste") { Id = grupoAdicionado.Id };
            ciclo.AtualizarGrupo(grupoAtualizado);
            Assert.Collection(ciclo.Grupos,
                grupo =>
                {
                    Assert.Equal("Teste", grupo.Nome);
                });
        }

        [Fact]
        public void RemoverGrupo_GrupoNaoEncontrado_ThrowsNotFoundException()
        {
            var ciclo = Factories.CriarCicloValido();
            var grupo = new Grupo("Grupo 1");
            ciclo.AdicionarGrupo(grupo);

            var exception = Assert.Throws<NotFoundException>(() => ciclo.RemoverGrupo(new Grupo("Teste")));
            Assert.Equal("Grupo não encontrado.", exception.Message);
            Assert.Equal(1, ciclo.Grupos.Count);
        }

        [Fact]
        public void RemoverGrupo_GrupoEncontrado_GrupoRemovido()
        {
            var ciclo = Factories.CriarCicloValido();
            var grupo1 = new Grupo("Grupo 1");
            ciclo.AdicionarGrupo(grupo1);

            var grupo2 = new Grupo("Grupo 2");
            ciclo.AdicionarGrupo(grupo2);

            var grupo3 = new Grupo("Grupo 3");
            ciclo.AdicionarGrupo(grupo3);

            var grupoRemovido = ciclo.RemoverGrupo(new Grupo() { Id = grupo2.Id });
            Assert.Equal(grupo2, grupoRemovido);
            Assert.Collection(ciclo.Grupos,
                grupo =>
                {
                    Assert.Equal(grupo1, grupo);
                },
                grupo =>
                {
                    Assert.Equal(grupo3, grupo);
                });
        }

        [Fact]
        public void AdicionarAssociado_AssociadoIdJaCadastrado_ThrowsCustomException()
        {
            var ciclo = Factories.CriarCicloValido();

            var grupo = new Grupo("Grupo 1");
            ciclo.AdicionarGrupo(grupo);
            var associado1 = new Associado(new Usuario(), grupo, 10, "1234");
            ciclo.AdicionarAssociado(associado1);

            var associado2 = new Associado(new Usuario(), grupo, 10, "1234") { Id = associado1.Id };
            var exception = Assert.Throws<CustomException>(() => ciclo.AdicionarAssociado(associado2));
            Assert.Equal("Esse associado já foi cadastrado nesse ciclo.", exception.Message);
        }

        [Fact]
        public void AdicionarAssociado_AssociadoCPFJaCadastrado_ThrowsCustomException()
        {
            var ciclo = Factories.CriarCicloValido();

            var grupo = new Grupo("Grupo 1");
            ciclo.AdicionarGrupo(grupo);
            var usuario1 = new Usuario("12312312312", "111", "Usuário 1", "Cargo 1", "Área 1", ciclo.Empresa);
            var associado1 = new Associado(usuario1, grupo, 10, "1234");
            ciclo.AdicionarAssociado(associado1);

            var usuario2 = new Usuario("12312312312", "222", "Usuário 2", "Cargo 2", "Área 2", ciclo.Empresa);
            var associado2 = new Associado(usuario2, grupo, 10, "1234");
            var exception = Assert.Throws<CustomException>(() => ciclo.AdicionarAssociado(associado2));
            Assert.Equal("Esse associado já foi cadastrado nesse ciclo.", exception.Message);
        }

        [Fact]
        public void AdicionarAssociado_EleicaoFinalizada_ThrowsCustomException()
        {
            var ciclo = Factories.CriarCicloValido();
            ciclo.Votacoes.First().IniciarVotacao();
            ciclo.Votacoes.First().FinalizarVotacao();
            var grupo = new Grupo("Grupo 1");
            var usuario1 = new Usuario("12312312312", "111", "Usuário 1", "Cargo 1", "Área 1", ciclo.Empresa);
            var associado1 = new Associado(usuario1, grupo, 10, "1234");
            var exception = Assert.Throws<CustomException>(() => ciclo.AdicionarAssociado(associado1));
            Assert.Equal("Não é possível adicionar associados após o término da 1ª eleição.", exception.Message);
        }

        [Fact]
        public void AdicionarAssociado_GrupoNaoCadastrado_ThrowsCustomException()
        {
            var ciclo = Factories.CriarCicloValido();
            var grupo = new Grupo("Grupo 1");
            var usuario1 = new Usuario("12312312312", "111", "Usuário 1", "Cargo 1", "Área 1", ciclo.Empresa);
            var associado1 = new Associado(usuario1, grupo, 10, "1234");
            var exception = Assert.Throws<CustomException>(() => ciclo.AdicionarAssociado(associado1));
            Assert.Equal("Esse associado deve estar em um grupo habilitado para esse ciclo.", exception.Message);
        }

        [Fact]
        public void AdicionarAssociado_ParametrosValidos_AssociadoAdicionadoALista()
        {
            var ciclo = Factories.CriarCicloValido();
            var grupo = new Grupo("Grupo 1");
            ciclo.AdicionarGrupo(grupo);
            var usuario1 = new Usuario("12312312312", "111", "Usuário 1", "Cargo 1", "Área 1", ciclo.Empresa);
            var associado1 = new Associado(usuario1, grupo, 10, "1234");
            Assert.Equal(0, ciclo.Associados.Count);
            ciclo.AdicionarAssociado(associado1);
            Assert.Collection(ciclo.Associados, associado => Assert.Equal(associado1, associado));
        }

        [Fact]
        public void RemoverAssociado_EleicaoFinalizada_ThrowsCustomException()
        {
            var ciclo = Factories.CriarCicloValido();

            ciclo.Votacoes.First().IniciarVotacao();
            var grupo = new Grupo("Grupo 1");
            ciclo.AdicionarGrupo(grupo);
            var usuario = new Usuario("12312312312", "111", "Usuário 1", "Cargo 1", "Área 1", ciclo.Empresa);
            var associado = new Associado(usuario, grupo, 10, "1234");

            ciclo.AdicionarAssociado(associado);

            ciclo.Votacoes.First().FinalizarVotacao();
            var exception = Assert.Throws<CustomException>(() => ciclo.RemoverAssociado(new Associado() { Id = associado.Id }));
            Assert.Equal("Não é possível remover associados após o término da 1ª eleição.", exception.Message);
        }

        [Fact]
        public void RemoverAssociado_AssociadoNaoCadastrado_ThrowsCustomException()
        {
            var ciclo = Factories.CriarCicloValido();

            var grupo = new Grupo("Grupo 1");
            ciclo.AdicionarGrupo(grupo);
            var usuario = new Usuario("12312312312", "111", "Usuário 1", "Cargo 1", "Área 1", ciclo.Empresa);
            var associado = new Associado(usuario, grupo, 10, "1234");

            ciclo.AdicionarAssociado(associado);

            var exception = Assert.Throws<CustomException>(() => ciclo.RemoverAssociado(new Associado() { Id = Guid.NewGuid() }));
            Assert.Equal("Associado não cadastrado nesse ciclo.", exception.Message);
        }

        [Fact]
        public void RemoverAssociado_AssociadoCadastrado_AssociadoRemovido()
        {
            var ciclo = Factories.CriarCicloValido();

            var grupo = new Grupo("Grupo 1");
            ciclo.AdicionarGrupo(grupo);
            var usuario = new Usuario("12312312312", "111", "Usuário 1", "Cargo 1", "Área 1", ciclo.Empresa);
            var associado = new Associado(usuario, grupo, 10, "1234");

            ciclo.AdicionarAssociado(associado);

            var associadoRemovido = ciclo.RemoverAssociado(new Associado() { Id = associado.Id });
            Assert.Equal(associado, associadoRemovido);
            Assert.Empty(ciclo.Associados);
        }

        [Fact]
        public void AtualizarAssociado_AssociadoNaoCadastrado_ThrowsNotFoundException()
        {
            var ciclo = Factories.CriarCicloValido();

            var grupo = new Grupo("Grupo 1");
            ciclo.AdicionarGrupo(grupo);
            var usuario = new Usuario("12312312312", "111", "Usuário 1", "Cargo 1", "Área 1", ciclo.Empresa);
            var associado = new Associado(usuario, grupo, 10, "1234");

            ciclo.AdicionarAssociado(associado);

            var exception = Assert.Throws<NotFoundException>(() => ciclo.AtualizarAssociado(new Associado() { Id = Guid.NewGuid() }));
            Assert.Equal("Associado não cadastrado nesse ciclo.", exception.Message);
        }

        [Fact]
        public void AtualizarAssociado_GrupoNaoCadastrado_ThrowsCustomException()
        {
            var ciclo = Factories.CriarCicloValido();

            var grupo = new Grupo("Grupo 1");
            ciclo.AdicionarGrupo(grupo);
            var usuario = new Usuario("12312312312", "111", "Usuário 1", "Cargo 1", "Área 1", ciclo.Empresa);
            var associado = new Associado(usuario, grupo, 10, "1234");
            var associadoAtualizado = new Associado(usuario, new Grupo("Teste"), 20, "4545") { Id = associado.Id };

            ciclo.AdicionarAssociado(associado);

            var exception = Assert.Throws<CustomException>(() => ciclo.AtualizarAssociado(associadoAtualizado));
            Assert.Equal("Esse grupo não está habilitado para esse ciclo.", exception.Message);
        }

        [Fact]
        public void AtualizarAssociado_ParametrosValidos_AtualizarAssociado()
        {
            var ciclo = Factories.CriarCicloValido();

            var grupo = new Grupo("Grupo 1");
            ciclo.AdicionarGrupo(grupo);
            var usuario1 = new Usuario("12312312312", "111", "Usuário 1", "Cargo 1", "Área 1", ciclo.Empresa);
            var associado1 = new Associado(usuario1, grupo, 10, "1234");
            ciclo.AdicionarAssociado(associado1);

            var usuario2 = new Usuario("12312312313", "222", "Usuário 2", "Cargo 2", "Área 2", ciclo.Empresa);
            var associado2 = new Associado(usuario2, grupo, 15, "4321");
            ciclo.AdicionarAssociado(associado2);

            var usuario3 = new Usuario("12312312314", "333", "Usuário 3", "Cargo 3", "Área 3", ciclo.Empresa);
            var associado3 = new Associado(usuario3, grupo, 24, "5454");
            ciclo.AdicionarAssociado(associado3);

            var novoGrupo = new Grupo("Teste");
            ciclo.AdicionarGrupo(novoGrupo);
            var usuarioAtualizado = new Usuario("12312312314", "333", "Usuário 3", "Cargo Alterado", "Área Alterada", ciclo.Empresa);
            var associadoAtualizado = new Associado(usuarioAtualizado, novoGrupo, 20, "4545") { Id = associado2.Id };
            ciclo.AtualizarAssociado(associadoAtualizado);

            Assert.Collection(ciclo.Associados,
                associado =>
                {
                    Assert.Equal("Cargo 1", associado.Cargo);
                    Assert.Equal("Área 1", associado.Area);
                    Assert.Equal(10, associado.Aplausogramas);
                    Assert.Equal(grupo, associado.Grupo);
                },
                associado =>
                {
                    Assert.Equal("Cargo Alterado", associado.Cargo);
                    Assert.Equal("Área Alterada", associado.Area);
                    Assert.Equal(20, associado.Aplausogramas);
                    Assert.Equal(novoGrupo, associado.Grupo);
                },
                associado =>
                {
                    Assert.Equal("Cargo 3", associado.Cargo);
                    Assert.Equal("Área 3", associado.Area);
                    Assert.Equal(24, associado.Aplausogramas);
                    Assert.Equal(grupo, associado.Grupo);
                });
        }

    }
}
