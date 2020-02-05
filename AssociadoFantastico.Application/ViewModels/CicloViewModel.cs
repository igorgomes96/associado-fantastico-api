using System;

namespace AssociadoFantastico.Application.ViewModels
{
    public class CicloViewModel: EntityViewModel
    {
        public int Ano { get; set; }
        public int Semestre { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFinalizacao { get; set; }


        public EmpresaViewModel Empresa { get; set; }
        public VotacaoViewModel VotacaoAssociadoFantastico { get; set; }
        public VotacaoViewModel VotacaoAssociadoSuperFantastico { get; set; }

    }
}
