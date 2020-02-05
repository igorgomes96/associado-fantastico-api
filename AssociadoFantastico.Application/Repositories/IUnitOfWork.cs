﻿using System;

namespace AssociadoFantastico.Application.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ICicloRepository CicloRepository { get; }
        IEmpresaRepository EmpresaRepository { get; }
        IGrupoRepository GrupoRepository { get; }
        IUsuarioRepository UsuarioRepository { get; }
        void Commit();
        void Rollback();
    }
}
