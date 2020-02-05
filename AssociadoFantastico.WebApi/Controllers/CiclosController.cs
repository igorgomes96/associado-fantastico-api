using AssociadoFantastico.Application.Exceptions;
using AssociadoFantastico.Application.Interfaces;
using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.WebApi.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AssociadoFantastico.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CiclosController : Controller<Ciclo, CicloViewModel>
    {
        public CiclosController(ICicloAppService appService) : base(appService)
        {
        }

        [HttpGet]
        [Pagination]
        public IEnumerable<CicloViewModel> Get(string status = "aberto")
        {
            var ciclos = (_appService as ICicloAppService).BuscarPelaEmpresa(EmpresaId)
                .OrderByDescending(c => c.Ano)
                .ThenByDescending(c => c.Semestre);
            
            if (status == "fechado")
                return ciclos.Where(c => c.DataFinalizacao.HasValue);

            return ciclos.Where(c => !c.DataFinalizacao.HasValue);
        }

        [HttpGet("{id}")]
        public CicloViewModel Get(Guid id)
        {
            var ciclo = _appService.BuscarPeloId(id);
            if (ciclo.Empresa.Id != EmpresaId) 
                throw new UnauthorizedException("Acesso não autorizado.");
            return ciclo;
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, CicloViewModel entity)
        {
            _appService.Atualizar(id, entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<CicloViewModel> Delete(Guid id) => _appService.Excluir(id);

        [HttpPost]
        public CicloViewModel PostNovoCiclo(NovoCicloViewModel novoCiclo)
        {
            novoCiclo.EmpresaId = EmpresaId;
            return (_appService as ICicloAppService).Adicionar(novoCiclo);
        }

        [HttpGet("{id}/votacoes/{votacaoId}")]
        public VotacaoViewModel GetVotacao(Guid id, Guid votacaoId) =>
            (_appService as ICicloAppService).BuscarVotacaoPeloId(id, votacaoId);

        [HttpPost("{id}/votacoes/{votacaoId}/iniciar")]
        public void PostIniciarVotacao(Guid id, Guid votacaoId) =>
            (_appService as ICicloAppService).IniciarVotacao(id, votacaoId);

        [HttpPost("{id}/votacoes/{votacaoId}/finalizar")]
        public void PostFinalizarVotacao(Guid id, Guid votacaoId) =>
            (_appService as ICicloAppService).FinalizarVotacao(id, votacaoId);

        [HttpPut("{id}/votacoes/{votacaoId}")]
        public void PutVotacao(Guid id, Guid votacaoId, VotacaoViewModel votacao) =>
            (_appService as ICicloAppService).AtualizarVotacao(id, votacaoId, votacao);
    }
}
