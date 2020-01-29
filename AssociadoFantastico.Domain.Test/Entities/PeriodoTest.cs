using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Domain.Exceptions;
using System;
using Xunit;

namespace AssociadoFantastico.Domain.Test.Entities
{
    public class PeriodoTest
    {
        [Fact]
        public void NovoPeriodo_DataFimMenorDataInicio_ThrowsCustomException()
        {
            var dataInicio = new DateTime(2020, 1, 2);
            var dataFim = new DateTime(2020, 1, 1);
            var exception = Assert.Throws<CustomException>(() => new Periodo(dataInicio, dataFim));
            Assert.Equal("A data de início deve ser maior que a data de fim.", exception.Message);
        }
    }
}
