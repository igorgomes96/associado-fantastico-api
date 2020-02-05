using System;

namespace AssociadoFantastico.Application.ViewModels
{
    public class VotacaoViewModel : EntityViewModel
    {
        public PeriodoViewModel PeriodoPrevisto { get; set; }
        public PeriodoViewModel PeriodoRealizado { get; set; }
        public Guid CicloId { get; set; }
    }
}
