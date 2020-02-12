using AssociadoFantastico.Application.EventsArgs;
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
                    .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.Usuario.Cpf))
                    .ForMember(dest => dest.Matricula, opt => opt.MapFrom(src => src.Usuario.Matricula))
                    .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Usuario.Nome))
                    .ForMember(dest => dest.EmpresaId, opt => opt.MapFrom(src => src.Usuario.EmpresaId))
                    .ForMember(dest => dest.Perfil, opt => opt.MapFrom(src => src.Usuario.Perfil));
                cfg.CreateMap<Elegivel, ElegivelViewModel>()
                    .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.Associado.Usuario.Cpf))
                    .ForMember(dest => dest.Matricula, opt => opt.MapFrom(src => src.Associado.Usuario.Matricula))
                    .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Associado.Usuario.Nome))
                    .ForMember(dest => dest.Cargo, opt => opt.MapFrom(src => src.Associado.Cargo))
                    .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Associado.Area))
                    .ForMember(dest => dest.CentroCusto, opt => opt.MapFrom(src => src.Associado.CentroCusto))
                    .ForMember(dest => dest.EmpresaId, opt => opt.MapFrom(src => src.Associado.Usuario.EmpresaId))
                    .ForMember(dest => dest.GrupoId, opt => opt.MapFrom(src => src.Associado.GrupoId))
                    .ForMember(dest => dest.GrupoNome, opt => opt.MapFrom(src => src.Associado.Grupo.Nome))
                    .ForMember(dest => dest.Perfil, opt => opt.MapFrom(src => src.Associado.Usuario.Perfil));
                cfg.CreateMap<FinalizacaoImportacaoStatusEventArgs, FinalizacaoImportacaoStatusViewModel>()
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString("g")));
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
