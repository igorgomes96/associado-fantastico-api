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
    public class EmpresasController : Controller<Empresa, EmpresaViewModel>
    {
        public EmpresasController(IEmpresaAppService appService) : base(appService)
        {
        }

        [HttpGet]
        [Pagination]
        public IEnumerable<EmpresaViewModel> Get() => _appService.BuscarTodos().AsQueryable();

        [HttpGet("{id}")]
        public EmpresaViewModel Get(Guid id) => _appService.BuscarPeloId(id);

        [HttpPost]
        public ActionResult<EmpresaViewModel> Post(EmpresaViewModel entity) => _appService.Adicionar(entity);

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, EmpresaViewModel entity)
        {
            _appService.Atualizar(id, entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<EmpresaViewModel> Delete(Guid id) => _appService.Excluir(id);
    }
}
