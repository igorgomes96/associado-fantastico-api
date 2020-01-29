using System.ComponentModel.DataAnnotations;

namespace AssociadoFantastico.Application.ViewModels
{
    public class GrupoViewModel: EntityViewModel
    {
        [Required(ErrorMessage = "O nome do grupo precisa ser informado.")]
        [StringLength(150, ErrorMessage = "O nome do grupo deve ter no máximo {1} caracteres.")]
        public string Nome { get; set; }

    }
}
