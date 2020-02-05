using AssociadoFantastico.Domain.Enums;
using System;

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

        public EApuracao Apuracao { get; private set; }
    }
}
