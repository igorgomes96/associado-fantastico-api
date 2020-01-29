namespace AssociadoFantastico.Application.ViewModels
{
    public abstract class VotacaoDetalhesViewModel : EntityViewModel
    {
        
        public PeriodoViewModel PeriodoPrevisto { get; set; }
        public PeriodoViewModel PeriodoRealizado { get; set; }

        public CicloViewModel Ciclo { get; set; }
    }
}
