using AssociadoFantastico.Application.Interfaces;
using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;
using AutoMapper;

namespace AssociadoFantastico.Application.Implementation
{
    public class GrupoAppService : AppServiceBase<Grupo, GrupoViewModel>, IGrupoAppService
    {
        public GrupoAppService(IUnitOfWork unitOfWork, IGrupoRepository repositoryBase, IMapper mapper) : base(unitOfWork, repositoryBase, mapper)
        {
        }
    }
}
