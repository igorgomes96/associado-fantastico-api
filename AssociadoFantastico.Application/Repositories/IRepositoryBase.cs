using System;
using System.Collections.Generic;

namespace AssociadoFantastico.Application.Repositories
{
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : class
    {
        TEntity Adicionar(TEntity obj);
        IEnumerable<TEntity> BuscarTodos();
        TEntity BuscarPeloId(Guid id);
        TEntity Atualizar(Guid id, TEntity obj);
        void Atualizar(TEntity obj);
        void Excluir(TEntity obj);
    }
}
