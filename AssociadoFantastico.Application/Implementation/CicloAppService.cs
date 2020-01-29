using AssociadoFantastico.Application.Exceptions;
using AssociadoFantastico.Application.Interfaces;
using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;
using AutoMapper;

namespace AssociadoFantastico.Application.Implementation
{
    public class CicloAppService : AppServiceBase<Ciclo, CicloViewModel>, ICicloAppService
    {
        public CicloAppService(IUnitOfWork unitOfWork, ICicloRepository repositoryBase, IMapper mapper) : base(unitOfWork, repositoryBase, mapper)
        {
        }

        public CicloViewModel Adicionar(NovoCicloViewModel novoCiclo)
        {
            var empresa = _unitOfWork.EmpresaRepository.BuscarPeloId(novoCiclo.EmpresaId);
            if (empresa == null) throw new NotFoundException("Empresa não encontrada.");
            var ciclo = new Ciclo(
                novoCiclo.Ano,
                novoCiclo.Semestre,
                novoCiclo.Descricao,
                _mapper.Map<Periodo>(novoCiclo.PeriodoVotacaoAssociadoFantastico),
                _mapper.Map<Periodo>(novoCiclo.PeriodoVotacaoAssociadoSuperFantastico),
                empresa);
            return base.Adicionar(ciclo);
        }
    }
}
