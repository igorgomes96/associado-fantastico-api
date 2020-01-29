using AssociadoFantastico.Application.Interfaces;
using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;
using AutoMapper;
using System.Linq;

namespace AssociadoFantastico.Application.Implementation
{
    public class UsuarioAppService : AppServiceBase<Usuario, UsuarioViewModel>, IUsuarioAppService
    {
        public UsuarioAppService(IUnitOfWork unitOfWork, IUsuarioRepository repositoryBase, IMapper mapper) : base(unitOfWork, repositoryBase, mapper)
        {
        }

        public UsuarioViewModel BuscarUsuario(string cpf, string matricula)
        {
            return _mapper.Map<UsuarioViewModel>(_repositoryBase.BuscarTodos()
                .SingleOrDefault(u => u.Cpf == cpf && u.Matricula == matricula));
        }
    }
}
