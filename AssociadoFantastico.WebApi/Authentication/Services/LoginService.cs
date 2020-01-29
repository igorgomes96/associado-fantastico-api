using AssociadoFantastico.Application.Interfaces;
using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Exceptions;
using AssociadoFantastico.WebApi.Enums;
using AssociadoFantastico.WebApi.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace AssociadoFantastico.WebApi.Authentication.Services
{
    public class LoginService: ILoginService
    {
        private readonly IUsuarioAppService _usuarioAppService;
        private readonly TokenConfigurations _tokenConfigurations;
        private readonly SigningConfigurations _signingConfigurations;
        public LoginService(
            IUsuarioAppService usuarioAppService,
            TokenConfigurations tokenConfigurations,
            SigningConfigurations signingConfigurations)
        {
            _usuarioAppService = usuarioAppService;
            _tokenConfigurations = tokenConfigurations;
            _signingConfigurations = signingConfigurations;
        }

        private UsuarioViewModel ValidaUsuario(string cpf, string matricula)
        {
            var usuario = _usuarioAppService.BuscarUsuario(cpf, matricula);
            if (usuario == null) throw new CustomException("Credenciais inválidas!");
            return usuario;
        }

        private ClaimsIdentity GeraIdentity(UsuarioViewModel usuario)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(usuario.Cpf, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(CustomClaimTypes.CPFUsuario, usuario.Cpf),
                        new Claim(CustomClaimTypes.UsuarioId, usuario.Id.ToString()),
                        new Claim(CustomClaimTypes.EmpresaId, usuario.EmpresaId.ToString()),
                        new Claim(ClaimTypes.Role, usuario.Perfil.ToString("g"))
                    }
                );
            return identity;
        }

        private AuthInfoViewModel GerarToken(UsuarioViewModel usuario, ClaimsIdentity identity)
        {
            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao + TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            var token = handler.WriteToken(securityToken);

            return new AuthInfoViewModel
            {
                AccessToken = token,
                Criacao = dataCriacao,
                Expiracao = dataExpiracao,
                Roles = identity.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToArray(),
                Matricula = usuario.Matricula,
                CPF = usuario.Cpf
            };
        }

        public AuthInfoViewModel Login(string email, string matricula)
        {
            var usuarioBanco = ValidaUsuario(email, matricula);
            var identity = GeraIdentity(usuarioBanco);
            return GerarToken(usuarioBanco, identity);
        }

    }
}
