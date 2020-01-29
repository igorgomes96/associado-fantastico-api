using AssociadoFantastico.Application.Exceptions;
using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace AssociadoFantastico.Infra.Data.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : Entity
    {
        protected readonly DbContext _db;
        protected RepositoryBase(DbContext db)
        {
            _db = db;
        }

        protected DbSet<TEntity> DbSet => _db.Set<TEntity>();

        public virtual TEntity Adicionar(TEntity obj) => DbSet.Add(obj).Entity;

        public virtual IEnumerable<TEntity> BuscarTodos() => DbSet;

        public virtual TEntity BuscarPeloId(Guid id) => DbSet.Find(id);

        public virtual void Excluir(TEntity obj) => DbSet.Remove(obj);

        public virtual void Atualizar(TEntity obj) => _db.Entry(obj).State = EntityState.Modified;

        public virtual TEntity Atualizar(Guid id, TEntity obj)
        {
            var entity = BuscarPeloId(id);
            if (entity == null) throw new NotFoundException("Código não encontrado.");
            obj.Id = id;
            _db.Entry(entity).CurrentValues.SetValues(obj);
            Atualizar(entity);
            return entity;
        }

        public virtual void Dispose() => _db.Dispose();

    }
}
