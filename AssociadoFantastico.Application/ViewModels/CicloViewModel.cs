using System.Collections.Generic;

namespace AssociadoFantastico.Application.ViewModels
{
    public class CicloViewModel: EntityViewModel
    {
        public int Ano { get; set; }
        public int Semestre { get; set; }
        public string Descricao { get; set; }

        public IEnumerable<VotacaoViewModel> Votacoes { get; set; }
    }
}
