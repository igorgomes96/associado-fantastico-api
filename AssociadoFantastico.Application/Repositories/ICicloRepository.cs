using AssociadoFantastico.Domain.Entities;
using System;
using System.Linq;

namespace AssociadoFantastico.Application.Repositories
{
    public interface ICicloRepository: IRepositoryBase<Ciclo>
    {
        Ciclo BuscarPeloPeriodo(Guid empresaId, int ano, int semestre);
        Ciclo BuscarCicloAnterior(Guid empresaId, int ano, int semestre);
        IQueryable<Associado> BuscarAssociados(Guid cicloId);
        IQueryable<Associado> BuscarAssociados(Guid cicloId, Guid grupoId);
        IQueryable<Elegivel> BuscarElegiveis(Guid cicloId, Guid votacaoId);
        IQueryable<Elegivel> BuscarElegiveis(Guid cicloId, Guid votacaoId, Guid grupoId);
    }
}
