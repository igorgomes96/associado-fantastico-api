using System.ComponentModel.DataAnnotations;

namespace AssociadoFantastico.Application.ViewModels
{
    public class EmpresaViewModel: EntityViewModel
    {
        [Required(ErrorMessage = "O nome da empresa precisa ser informado.")]
        [StringLength(100, ErrorMessage = "O nome da empresa precisa ter no máximo {1} caracteres.")]
        public string Nome { get; set; }
    }
}
