using AssociadoFantastico.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace AssociadoFantastico.Domain.Entities
{
    public class Dimensionamento : ValueObject
    {
        public Dimensionamento(int intervalo, int acrescimo)
        {
            Intervalo = intervalo;
            Acrescimo = acrescimo;

            if (intervalo < 0 || acrescimo < 0)
                throw new CustomException("O intervalo e/ou acréscimo do dimensionamento não podem ser menor que 0.");
        }

        public int Intervalo { get; private set; }  // Se 0, valor fixo independente da quantidade de associados
        public int Acrescimo { get; private set; }

        public int CalcularQuantidade(int qtdaAssociados)
        {
            if (Intervalo == 0) return Acrescimo;
            return (int)Math.Round((decimal)qtdaAssociados / Intervalo, MidpointRounding.AwayFromZero) * Acrescimo;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Intervalo;
            yield return Acrescimo;
        }

    }
}
