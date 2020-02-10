using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;
using AutoMapper;
using System.Linq;

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
                cfg.CreateMap<Ciclo, CicloViewModel>()
                    .ForMember(
                        dest => dest.VotacaoAssociadoFantastico,
                        src => src.MapFrom(ciclo => ciclo.Votacoes.Single(v => v is VotacaoAssociadoFantastico))
                    ).ForMember(
                        dest => dest.VotacaoAssociadoSuperFantastico,
                        src => src.MapFrom(ciclo => ciclo.Votacoes.Single(v => v is VotacaoAssociadoSuperFantastico))
                    );
                cfg.CreateMap<Periodo, PeriodoViewModel>().ReverseMap();
                cfg.CreateMap<Usuario, UsuarioViewModel>();
                cfg.CreateMap<Associado, AssociadoViewModel>()
                    .ConstructUsing(src => new AssociadoViewModel(
                        src.Usuario.Cpf,
                        src.Usuario.Matricula,
                        src.Usuario.Nome,
                        src.Cargo,
                        src.Area,
                        src.CentroCusto,
                        src.Aplausogramas,
                        src.Usuario.Perfil,
                        src.GrupoId));
                cfg.CreateMap<Elegivel, ElegivelViewModel>()
                    .ConstructUsing(src => new ElegivelViewModel(
                        src.Associado.Usuario.Cpf,
                        src.Associado.Usuario.Matricula,
                        src.Associado.Usuario.Nome,
                        src.Associado.Cargo,
                        src.Associado.Area,
                        src.Associado.CentroCusto,
                        src.Associado.Aplausogramas,
                        src.Associado.Usuario.Perfil,
                        src.Associado.GrupoId));
                cfg.CreateMap<Grupo, GrupoViewModel>().ReverseMap().ConstructUsing(dest => new Grupo(dest.Nome));
                cfg.CreateMap<Votacao, VotacaoViewModel>()
                    .ForMember(
                        dest => dest.TipoVotacao, 
                        src => src.MapFrom(v => 
                            v is VotacaoAssociadoFantastico 
                            ? TipoVotacao.VotacaoAssociadoFantastico 
                            : TipoVotacao.VotacaoAssociadoSuperFantastico)
                        );
                cfg.CreateMap<Voto, VotoViewModel>();
                cfg.CreateMap<Importacao, ImportacaoViewModel>()
                    .ForMember(dest => dest.Horario, src => src.MapFrom(i => i.DataCadastro));
            });
            return config.CreateMapper();
        }
    }
}
