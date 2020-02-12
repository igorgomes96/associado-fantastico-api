using AssociadoFantastico.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AssociadoFantastico.WebApi.BackgroundTasks
{
    public class ImportacaoAssociadosHostedService : ScopedProcessor
    {
        public ImportacaoAssociadosHostedService(IServiceProvider serviceScopeFactory, ILogger<ImportacaoAssociadosHostedService> logger) : base(serviceScopeFactory, logger) { }

        public override Task ProcessInScope(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IImportacaoAssociadosAppService>();
                scopedProcessingService.RealizarImportacaoEmBrackground();
            }
            return Task.CompletedTask;
        }

        protected override int Delay { get { return (int)TimeSpan.FromSeconds(10).TotalMilliseconds; } }
    }
}
