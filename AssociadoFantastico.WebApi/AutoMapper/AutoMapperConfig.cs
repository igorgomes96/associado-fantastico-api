using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;
using AutoMapper;

namespace AssociadoFantastico.WebApi.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static IMapper MapperConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Empresa, EmpresaViewModel>().ReverseMap()
                    .ConvertUsing(dest => new Empresa(dest.Nome));
                cfg.CreateMap<Ciclo, CicloViewModel>();
                cfg.CreateMap<Periodo, PeriodoViewModel>().ReverseMap();
                cfg.CreateMap<Usuario, UsuarioViewModel>();
                cfg.CreateMap<Associado, AssociadoViewModel>()
                    .ConstructUsing(src => new AssociadoViewModel(
                        src.Usuario.Cpf,
                        src.Usuario.Matricula,
                        src.Usuario.Nome,
                        src.Usuario.Cargo,
                        src.Usuario.Area,
                        src.Usuario.Perfil,
                        src.GrupoId));
                cfg.CreateMap<Elegivel, ElegivelViewModel>()
                    .ConstructUsing(src => new ElegivelViewModel(
                        src.Associado.Usuario.Cpf,
                        src.Associado.Usuario.Matricula,
                        src.Associado.Usuario.Nome,
                        src.Associado.Usuario.Cargo,
                        src.Associado.Usuario.Area,
                        src.Associado.Usuario.Perfil,
                        src.Associado.GrupoId));
                cfg.CreateMap<Grupo, GrupoViewModel>().ReverseMap().ConstructUsing(dest => new Grupo(dest.Nome));
                cfg.CreateMap<Votacao, VotacaoViewModel>();
                cfg.CreateMap<Votacao, VotacaoDetalhesViewModel>();
                cfg.CreateMap<Voto, VotoViewModel>();
            });
            return config.CreateMapper();
        }
    }
}
