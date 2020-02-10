using AssociadoFantastico.Domain.Entities;
using System;

namespace AssociadoFantastico.Application.EventsArgs
{
    public class FinalizacaoImportacaoStatusEventArgs : EventArgs
    {
        public StatusImportacao Status { get; set; }
        public int QtdaErros { get; set; }
        public string CPFUsuario { get; set; }
    }
}
