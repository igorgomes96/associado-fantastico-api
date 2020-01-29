using System;
using System.ComponentModel.DataAnnotations;

namespace AssociadoFantastico.Application.ViewModels
{
    public class NovoCicloViewModel: EntityViewModel
    {
        [Required(ErrorMessage = "O ano do ciclo precisa ser informado.")]
        public int Ano { get; set; }
        [Required(ErrorMessage = "O semestre do ciclo precisa ser informado.")]
        public int Semestre { get; set; }
        [Required(ErrorMessage = "A descrição do ciclo precisa ser informada.")]
        [StringLength(255, ErrorMessage = "A descrição deve conter no máximo {1} caracteres.")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "O período previsto para a votação dos associados fantásticos precisa ser informado.")]
        public PeriodoViewModel PeriodoVotacaoAssociadoFantastico { get; set; }
        [Required(ErrorMessage = "O período previsto para a votação dos associados super fantásticos precisa ser informado.")]
        public PeriodoViewModel PeriodoVotacaoAssociadoSuperFantastico { get; set; }
        public Guid EmpresaId { get; set; }

    }
}
