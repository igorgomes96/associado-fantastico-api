
using AssociadoFantastico.Application.Interfaces;
using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;
using AutoMapper;

namespace AssociadoFantastico.Application.Implementation
{
    public class EmpresaAppService : AppServiceBase<Empresa, EmpresaViewModel>, IEmpresaAppService
    {
        public EmpresaAppService(IUnitOfWork unitOfWork, IEmpresaRepository repositoryBase, IMapper mapper) : base(unitOfWork, repositoryBase, mapper, "Empresa", 'a')
        {
        }
    }
}
