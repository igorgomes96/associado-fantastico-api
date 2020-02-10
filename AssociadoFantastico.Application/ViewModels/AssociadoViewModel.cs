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
            string centroCusto,
            int aplausogramas,
            EPerfilUsuario perfil,
            Guid grupoId) : base(cpf, matricula, nome, cargo, area, perfil)
        {
            CentroCusto = centroCusto;
            GrupoId = grupoId;
            Aplausogramas = aplausogramas;
        }

        [Required(ErrorMessage = "O grupo do associado precisa ser informado.")]
        public Guid GrupoId { get; set; }
        public string GrupoNome { get; set; }
        [Range(0, 100000, ErrorMessage = "A quantidade de aplausogramas não pode ser menor que {1}.")]
        public int Aplausogramas { get; set; }
        [Required(ErrorMessage = "O centro de custo do associado precisa ser informado.")]
        [StringLength(100, ErrorMessage = "O centro de custo deve conter no máximo {1} caracteres.")]
        public string CentroCusto { get; set; }
        public Guid UsuarioId { get; set; }
    }
}
