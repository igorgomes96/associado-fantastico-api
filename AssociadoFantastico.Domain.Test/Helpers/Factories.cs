using AssociadoFantastico.Domain.Entities;
using System;

namespace AssociadoFantastico.Domain.Test.Helpers
{
    public static class Factories
    {
        public static Ciclo CriarCicloValido()
        {
            var empresa = new Empresa("Empresa teste");
            var periodo1Inicio = new DateTime(2020, 1, 1);
            var periodo1Fim = new DateTime(2020, 1, 2);
            var periodo2Inicio = new DateTime(2020, 1, 3);
            var periodo2Fim = new DateTime(2020, 1, 4);
            var periodo1 = new Periodo(periodo1Inicio, periodo1Fim);
            var periodo2 = new Periodo(periodo2Inicio, periodo2Fim);
            return new Ciclo(2020, 1, "Teste", periodo1, periodo2, empresa);
        }
    }
}
