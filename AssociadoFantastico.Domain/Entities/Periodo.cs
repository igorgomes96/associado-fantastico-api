using AssociadoFantastico.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace AssociadoFantastico.Domain.Entities
{
    public class Periodo : ValueObject
    {
        public Periodo(DateTime? dataInicio, DateTime? dataFim = null)
        {
            if (dataInicio.HasValue && dataFim.HasValue && (dataInicio.Value >= dataFim.Value))
                throw new CustomException("A data de início deve ser maior que a data de fim.");

            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return DataInicio;
            yield return DataFim;
        }
    }
}
