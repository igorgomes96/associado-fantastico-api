using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AssociadoFantastico.Infra.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly AssociadoFantasticoContext Context;
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(IServiceProvider serviceProvider, AssociadoFantasticoContext context)
        {
            Context = context;
            _serviceProvider = serviceProvider;
        }

        public void Commit()
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch
                {
                    //Log Exception Handling message                      
                    dbContextTransaction.Rollback();
                    throw;
                }
            }

        }

        public void Rollback()
        {
            var changedEntries = Context.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }
        #region Public Properties  
        public IUsuarioRepository UsuarioRepository => (IUsuarioRepository)_serviceProvider.GetService(typeof(IUsuarioRepository));
        public IEmpresaRepository EmpresaRepository => (IEmpresaRepository)_serviceProvider.GetService(typeof(IEmpresaRepository));
        public ICicloRepository CicloRepository => (ICicloRepository)_serviceProvider.GetService(typeof(ICicloRepository));
        public IImportacaoRepository ImportacaoRepository => (IImportacaoRepository)_serviceProvider.GetService(typeof(IImportacaoRepository));


        #endregion


        #region IDisposable Support  
        private bool _disposedValue = false; // To detect redundant calls  

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing)
            {
                //dispose managed state (managed objects).  
            }

            // free unmanaged resources (unmanaged objects) and override a finalizer below.  
            // set large fields to null.  

            _disposedValue = true;
        }

        // override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.  
        // ~UnitOfWork() {  
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.  
        //   Dispose(false);  
        // }  

        // This code added to correctly implement the disposable pattern.  
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.  
            Dispose(true);
            // uncomment the following line if the finalizer is overridden above.  
            // GC.SuppressFinalize(this);  
        }
        #endregion

    }
}