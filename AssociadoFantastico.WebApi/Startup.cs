using AssociadoFantastico.Application.EventsArgs;
using AssociadoFantastico.Application.Interfaces;
using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Infra.Data.Context;
using AssociadoFantastico.WebApi.Extensions;
using AssociadoFantastico.WebApi.SignalR;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AssociadoFantastico.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AssociadoFantasticoContext>(opt =>
            {
                opt.UseLazyLoadingProxies();
                opt.UseMySql(Configuration.GetConnectionString("MySqlConnection"),
                    b => b.MigrationsAssembly("AssociadoFantastico.WebApi"));
            });

            services.AddDependecies();
            services.AddAuthenticationConfiguration(Configuration);
            services.AddDimensionamentosPadroes(Configuration);
            services.AddImportacaoConfiguration(Configuration);
            services.AddBackgroundTasks();

            services.AddAutoMapper();

            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            services.AddCors(options => options.AddPolicy("AllowAll", builder =>
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("http://localhost:4200")
                    .AllowCredentials()));

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IHubContext<ProgressHub> hubContext,
            IProgressoImportacaoEvent notificacaoProgressoEvent,
            IMapper mapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseCors("AllowAll");
            app.UseSignalR(routes =>
            {
                routes.MapHub<ProgressHub>("/api/signalr");
            });
            app.UseAuthentication();

            app.UseHttpErrorMiddleware();


            notificacaoProgressoEvent.NotificacaoProgresso += (object sender, ProgressoImportacaoEventArgs args) =>
            {
                hubContext.Clients.User(args.CPFUsuario).SendAsync("progressoimportacao", args);
            };

            notificacaoProgressoEvent.ImportacaoFinalizada += (object sender, FinalizacaoImportacaoStatusEventArgs args) =>
            {
                hubContext.Clients.User(args.CPFUsuario).SendAsync("importacaofinalizada", mapper.Map<FinalizacaoImportacaoStatusViewModel>(args));
            };

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
