using AssociadoFantastico.Domain.Entities;
using System;
using System.Collections.Generic;

namespace AssociadoFantastico.Application.Interfaces
{
    public interface IImportacaoAppService
    {
        void RealizarImportacaoEmBrackground();
        IEnumerable<Inconsistencia> RetornarInconsistenciasDaImportacao(Guid id);
    }
}
