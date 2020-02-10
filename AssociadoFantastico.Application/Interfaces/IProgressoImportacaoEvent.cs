using AssociadoFantastico.Application.EventsArgs;
using System;

namespace AssociadoFantastico.Application.Interfaces
{
    public interface IProgressoImportacaoEvent : IDisposable
    {
        event EventHandler<ProgressoImportacaoEventArgs> NotificacaoProgresso;
        event EventHandler<FinalizacaoImportacaoStatusEventArgs> ImportacaoFinalizada;
        void OnNotificacaoProgresso(object sender, ProgressoImportacaoEventArgs e);
        void OnImportacaoFinalizada(object sender, FinalizacaoImportacaoStatusEventArgs e);
    }
}
