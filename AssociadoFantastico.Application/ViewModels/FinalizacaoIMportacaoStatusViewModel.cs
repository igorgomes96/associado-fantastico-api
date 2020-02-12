using System;
using System.Collections.Generic;
using System.Text;

namespace AssociadoFantastico.Application.ViewModels
{
    public class FinalizacaoImportacaoStatusViewModel
    {
        public string Status { get; set; }
        public int QtdaErros { get; set; }
        public string CPFUsuario { get; set; }
    }
}
