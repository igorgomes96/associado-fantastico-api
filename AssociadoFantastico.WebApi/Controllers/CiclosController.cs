using AssociadoFantastico.Application.Exceptions;
using AssociadoFantastico.Application.Interfaces;
using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.WebApi.Filters;
using Cipa.WebApi.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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

        #region Ciclos
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
        #endregion

        #region Votações
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

        [HttpPost("{id}/votacoes/{votacaoId}/importarassociados"), DisableRequestSizeLimit]
        public ActionResult<ImportacaoViewModel> ImportarAssociados(Guid id, Guid votacaoId)
        {
            if (Request.Form.Files == null || Request.Form.Files.Count == 0)
                return BadRequest("Nenhum arquivo foi enviado.");

            if (Request.Form.Files.Count > 1)
                return BadRequest("Somente um arquivo pode ser enviado.");

            var formFile = Request.Form.Files.First();
            var fileName = formFile.FileName;
            byte[] arquivo = null;
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                arquivo = ms.ToArray();
            }
            return (_appService as ICicloAppService).ImportarAssociados(id, votacaoId, arquivo, fileName, CPFUsuario);
        }
        #endregion

        #region Grupos
        [HttpGet("{id}/grupos")]
        public IEnumerable<GrupoViewModel> GetGrupos(Guid id) =>
            (_appService as ICicloAppService).BuscarGrupos(id);

        [HttpPost("{id}/grupos")]
        public void AdicionarGrupo(Guid id, GrupoViewModel grupo) =>
            (_appService as ICicloAppService).AdicionarGrupo(id, grupo);

        [HttpPut("{id}/grupos/{grupoId}")]
        public void AtualizarGrupo(Guid id, Guid grupoId, GrupoViewModel grupo)
        {
            grupo.Id = grupoId;
            (_appService as ICicloAppService).AtualizarGrupo(id, grupo);
        }

        [HttpDelete("{id}/grupos/{grupoId}")]
        public GrupoViewModel RemoverGrupo(Guid id, Guid grupoId) => 
            (_appService as ICicloAppService).RemoverGrupo(id, grupoId);
        #endregion

        #region Associados
        [Query("Nome")]
        [Pagination]
        [HttpGet("{id}/associados")]
        public IEnumerable<AssociadoViewModel> GetAssociados(Guid id, Guid? grupoId = null) =>
            (_appService as ICicloAppService).BuscarAssociados(id, grupoId);


        [Query("Nome")]
        [Pagination]
        [HttpGet("{id}/associados/naoelegiveis")]
        public IEnumerable<AssociadoViewModel> GetAssociadosNaoElegiveis(Guid id, Guid? grupoId = null) =>
           (_appService as ICicloAppService).BuscarAssociadosNaoElegiveis(id, grupoId);


        [HttpGet("{id}/associados/{associadoId}")]
        public AssociadoViewModel GetAssociado(Guid id, Guid associadoId) =>
            (_appService as ICicloAppService).BuscarAssociado(id, associadoId);


        [HttpPut("{id}/associados/{associadoId}")]
        public void PutAtualizarAssociado(Guid id, Guid associadoId, AssociadoViewModel associado)
        {
            associado.Id = associadoId;
            associado.EmpresaId = EmpresaId;
            (_appService as ICicloAppService).AtualizarAssociado(id, associado);
        }

        [HttpDelete("{id}/associados/{associadoId}")]
        public void DeleteAssociado(Guid id, Guid associadoId) =>
            (_appService as ICicloAppService).RemoverAssociado(id, associadoId);

        [HttpPost("{id}/associados")]
        public void AdicionarAssociado(Guid id, AssociadoViewModel associado)
        {
            associado.EmpresaId = EmpresaId;
            (_appService as ICicloAppService).AdicionarAssociado(id, associado);
        }
        #endregion

        #region Elegíveis
        [Query("Nome")]
        [HttpGet("{id}/votacoes/{votacaoId}/elegiveis")]
        public IEnumerable<ElegivelViewModel> GetElegiveis(Guid id, Guid votacaoId, Guid? grupoId = null) =>
            (_appService as ICicloAppService).BuscarElegiveis(id, votacaoId, grupoId);

        [HttpPost("{id}/votacoes/{votacaoId}/elegiveis")]
        public ElegivelViewModel PostElegivel(Guid id, Guid votacaoId, Associado associado) =>
            (_appService as ICicloAppService).AdicionarElegivel(id, votacaoId, associado.Id);

        [HttpDelete("{id}/votacoes/{votacaoId}/elegiveis/{elegivelId}")]
        public ElegivelViewModel PostElegivel(Guid id, Guid votacaoId, Guid elegivelId) =>
            (_appService as ICicloAppService).RemoverElegivel(id, votacaoId, elegivelId);

        [HttpPost("{id}/votacoes/{votacaoId}/elegiveis/{elegivelId}/foto"), DisableRequestSizeLimit]
        public ActionResult<ElegivelViewModel> PostFotoElegivel(Guid id, Guid votacaoId, Guid elegivelId)
        {
            if (Request.Form.Files == null || Request.Form.Files.Count == 0)
                return BadRequest("Nenhuma foto foi enviada.");

            if (Request.Form.Files.Count > 1)
                return BadRequest("Somente uma foto pode ser enviada.");

            var formFile = Request.Form.Files.First();
            var fileName = formFile.FileName;
            byte[] foto = null;
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                foto = ms.ToArray();
            }
            return (_appService as ICicloAppService).AtualizarFotoElegivel(id, votacaoId, elegivelId, foto, fileName);
        }

        [HttpGet("{id}/votacoes/{votacaoId}/elegiveis/{elegivelId}/foto")]
        public IActionResult GetFotoElegivel(Guid id, Guid votacaoId, Guid elegivelId)
        {
            return new FileStreamResult((_appService as ICicloAppService).BuscarFotoElegivel(id, votacaoId, elegivelId), "image/jpeg");
        }
        #endregion
    }
}
