using AssociadoFantastico.Application.Interfaces;
using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Domain.Enums;
using AssociadoFantastico.WebApi.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace AssociadoFantastico.WebApi.Controllers
{
    public class Controller<TEntity, TEntityDTO> : ControllerBase where TEntity: Entity where TEntityDTO : EntityViewModel
    {
        protected readonly IAppServiceBase<TEntity, TEntityDTO> _appService;

        protected Controller(IAppServiceBase<TEntity, TEntityDTO> appService)
        {
            _appService = appService;
        }

        protected string IpRequisicao
        {
            get
            {
                return Request.HttpContext.Connection.RemoteIpAddress.ToString();
            }
        }

        protected Guid EmpresaId
        {
            get
            {
                return Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.EmpresaId).Value);
            }
        }

        protected Guid UsuarioId
        {
            get
            {
                return Guid.Parse(User.Claims.First(c => c.Type == CustomClaimTypes.UsuarioId).Value);
            }
        }

        protected string CPFUsuario
        {
            get
            {
                return User.Identity.Name;
            }
        }

        protected bool UsuarioEhDoRH => User.IsInRole(EPerfilUsuario.RH.ToString("g"));
        protected bool UsuarioEhAssociado => User.IsInRole(EPerfilUsuario.Associado.ToString("g"));
        protected bool UsuarioEhCoordenador => User.IsInRole(EPerfilUsuario.Coordenador.ToString("g"));


    }
}