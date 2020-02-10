using AssociadoFantastico.Application.Configurations;
using AssociadoFantastico.Application.Implementation;
using AssociadoFantastico.Application.Interfaces;
using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Infra.Data.Repositories;
using AssociadoFantastico.WebApi.Authentication;
using AssociadoFantastico.WebApi.Authentication.Services;
using AssociadoFantastico.WebApi.AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AssociadoFantastico.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            return services.AddSingleton(AutoMapperConfig.MapperConfig());
        }

        public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration Configuration)
        {
            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => JwtBearerOptionsConfig.JwtConfiguration(options, signingConfigurations, tokenConfigurations));

            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddDimensionamentosPadroes(this IServiceCollection services, IConfiguration Configuration)
        {
            var intervalo = Configuration.GetValue<int>("DimensionamentoPadrao:VotacaoAssociadoFantastico:Intervalo");
            var acrescimo = Configuration.GetValue<int>("DimensionamentoPadrao:VotacaoAssociadoFantastico:Acrescimo");

            var dimensionamentoAssociadoFantastico = new DimensionamentoPadraoAssociadoFantastico(intervalo, acrescimo);
            services.AddSingleton(dimensionamentoAssociadoFantastico);

            intervalo = Configuration.GetValue<int>("DimensionamentoPadrao:VotacaoAssociadoSuperFantastico:Intervalo");
            acrescimo = Configuration.GetValue<int>("DimensionamentoPadrao:VotacaoAssociadoSuperFantastico:Acrescimo");

            var dimensionamentoAssociadoSuperFantastico = new DimensionamentoPadraoAssociadoSuperFantastico(intervalo, acrescimo);
            services.AddSingleton(dimensionamentoAssociadoSuperFantastico);

            return services;
        }

        public static IServiceCollection AddDependecies(this IServiceCollection services)
        {
            // Unit Of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Ciclos
            services.AddScoped<ICicloAppService, CicloAppService>();
            services.AddScoped<ICicloRepository, CicloRepository>();

            // Empresas
            services.AddScoped<IEmpresaAppService, EmpresaAppService>();
            services.AddScoped<IEmpresaRepository, EmpresaRepository>();

            // Usuarios
            services.AddScoped<IUsuarioAppService, UsuarioAppService>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            // Importações
            //services.AddScoped<IImportacaoAppService, UsuarioAppService>();
            services.AddScoped<IImportacaoRepository, ImportacaoRepository>();

            // Login
            services.AddScoped<ILoginService, LoginService>();

            return services;
        }
    }
}
