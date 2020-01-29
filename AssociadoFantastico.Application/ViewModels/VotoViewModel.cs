using System;

namespace AssociadoFantastico.Application.ViewModels
{
    public class VotoViewModel : EntityViewModel
    {
        public Guid AssociadoId { get; set; }
        public DateTime Horario { get; set; }
        public string Ip { get; set; }

        public virtual AssociadoViewModel Associado { get; set; }
    }
}
