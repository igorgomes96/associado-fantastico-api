using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;
using System;
using System.Collections.Generic;

namespace AssociadoFantastico.Application.Interfaces
{
    public interface IAppServiceBase<TEntity, TEntityDTO> : IDisposable where TEntity : Entity where TEntityDTO : EntityViewModel
    {
        TEntityDTO Adicionar(TEntityDTO obj);
        TEntityDTO Adicionar(TEntity entity);
        TEntityDTO BuscarPeloId(Guid id);
        IEnumerable<TEntityDTO> BuscarTodos();
        void Atualizar(Guid id, TEntityDTO obj);
        TEntityDTO Excluir(Guid id);
        TEntityDTO Excluir(TEntity obj);
    }
}
