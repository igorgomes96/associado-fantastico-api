using AssociadoFantastico.Application.Interfaces;
using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AssociadoFantastico.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CiclosController : Controller<Ciclo, CicloViewModel>
    {
        public CiclosController(ICicloAppService appService) : base(appService)
        {
        }

        [HttpGet]
        [Pagination]
        public IEnumerable<CicloViewModel> Get() => _appService.BuscarTodos().AsQueryable();

        [HttpGet("{id}")]
        public CicloViewModel Get(Guid id) => _appService.BuscarPeloId(id);

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
    }
}
