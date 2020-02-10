using AssociadoFantastico.Application.Services.Models;

namespace AssociadoFantastico.Application.Interfaces
{
    public interface IImportacaoServiceConfiguration
    {
        DataColumnValidator[] Validators { get; }
    }
}
