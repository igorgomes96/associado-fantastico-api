using AssociadoFantastico.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace AssociadoFantastico.Application.ViewModels
{
    public class ElegivelViewModel: AssociadoViewModel
    {
        public ElegivelViewModel(
            string cpf,
            string matricula,
            string nome,
            string cargo,
            string area,
            EPerfilUsuario perfil,
            Guid grupoId) : base(cpf, matricula, nome, cargo, area, perfil, grupoId)
        { }

        [Range(0, 100000, ErrorMessage = "A quantidade de aplausogramas não pode ser menor que {1}.")]
        public int Aplausogramas { get; set; }
    }
}
