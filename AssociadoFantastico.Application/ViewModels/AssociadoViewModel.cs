using AssociadoFantastico.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace AssociadoFantastico.Application.ViewModels
{
    public class AssociadoViewModel: UsuarioViewModel
    {
        public AssociadoViewModel(
            string cpf,
            string matricula,
            string nome,
            string cargo,
            string area,
            EPerfilUsuario perfil,
            Guid grupoId) : base(cpf, matricula, nome, cargo, area, perfil)
        {
            GrupoId = grupoId;
        }

        [Required(ErrorMessage = "O grupo do associado precisa ser informado.")]
        public Guid GrupoId { get; set; }
    }
}
