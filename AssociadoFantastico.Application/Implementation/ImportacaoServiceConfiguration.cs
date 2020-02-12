using AssociadoFantastico.Application.Interfaces;
using AssociadoFantastico.Application.Services.Models;

namespace AssociadoFantastico.Application.Implementation
{
    public class ImportacaoServiceConfiguration : IImportacaoServiceConfiguration
    {
        public DataColumnValidator[] Validators { get; set; }
    }
}
