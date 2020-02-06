using AssociadoFantastico.Domain.Entities;
using System;

namespace AssociadoFantastico.Application.Repositories
{
    public interface ICicloRepository: IRepositoryBase<Ciclo>
    {
        Ciclo BuscarPeloPeriodo(Guid empresaId, int ano, int semestre);
        Ciclo BuscarCicloAnterior(Guid empresaId, int ano, int semestre);
    }
}
