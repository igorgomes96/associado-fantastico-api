using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using AssociadoFantastico.Application.Interfaces;
using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Application.Services.Interfaces;
using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;
using AssociadoFantastico.Domain.Enums;
using AutoMapper;
using static AssociadoFantastico.Application.Helpers.Constants;

namespace AssociadoFantastico.Application.Implementation
{
    public class ImportacaoAssociadosAppService : ImportacaoAppService
    {
        public ImportacaoAssociadosAppService(
            IUnitOfWork unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IImportacaoServiceConfiguration importacaoConfiguration,
            IProgressoImportacaoEvent progressoEvent) : base(unitOfWork, excelService, mapper, importacaoConfiguration, progressoEvent)
        { }

        protected override void ProcessarDados(Importacao importacao, DataTable dataTable)
        {
            var associados = AdicionarAssociados(dataTable);
            var inconsistencias = RetornarInconsistenciasCPFsDuplicados(associados).ToList();

            if (!FinalizarImportacaoComErro(importacao, inconsistencias))
            {
                int linha = LINHA_INICIAL_ARQUIVO;
                foreach (var associadoViewModel in associados)
                {
                    var usuario = _unitOfWork.UsuarioRepository.BuscarPeloCPF(associadoViewModel.Cpf);
                    if (usuario == null)
                        usuario = new Usuario(
                            associadoViewModel.Cpf,
                            associadoViewModel.Matricula,
                            associadoViewModel.Nome,
                            associadoViewModel.Cargo,
                            associadoViewModel.Area,
                            importacao.Votacao.Ciclo.Empresa);

                    var grupo = importacao.Votacao.Ciclo.BuscarGrupoPeloNome(associadoViewModel.GrupoNome);
                    if (grupo == null)
                    {
                        inconsistencias.Add(new Inconsistencia(ColunasArquivo.Grupo, linha, $"Grupo não encontrado: {associadoViewModel.GrupoNome}."));
                    }
                    else
                    {
                        var associado = new Associado(usuario, grupo, associadoViewModel.Aplausogramas, associadoViewModel.CentroCusto);
                        var associadoExistente = importacao.Votacao.Ciclo.BuscarAssociadoPeloCPF(associadoViewModel.Cpf);
                        if (associadoExistente == null)
                            importacao.Votacao.Ciclo.AdicionarAssociado(associado);
                        else
                            associadoExistente.AtualizarDados(associado);
                    }
                    NotificarProgresso(linha - LINHA_INICIAL_ARQUIVO + 1, associados.Count, importacao.CPFUsuarioImportacao);
                    linha++;
                }
                if (!FinalizarImportacaoComErro(importacao, inconsistencias)) FinalizarImportacaoComSucesso(importacao);
            }
        }

        private List<AssociadoViewModel> AdicionarAssociados(DataTable dataTable)
        {
            List<AssociadoViewModel> associados = new List<AssociadoViewModel>();
            int linha = LINHA_INICIAL_ARQUIVO;
            var validators = _dataColumnValidators.ToDictionary(k => k.ColumnName);
            foreach (DataRow dr in dataTable.Rows)
            {
                var grupoNome = ObtemValorFormatoCorreto<string>(dr, ColunasArquivo.Grupo, validators[ColunasArquivo.Grupo]).Trim();
                var nome = ObtemValorFormatoCorreto<string>(dr, ColunasArquivo.Nome, validators[ColunasArquivo.Nome]).Trim();
                var matricula = ObtemValorFormatoCorreto<string>(dr, ColunasArquivo.Matricula, validators[ColunasArquivo.Matricula]).Trim();
                var cpf = Regex.Match(ObtemValorFormatoCorreto<string>(dr, ColunasArquivo.CPF, validators[ColunasArquivo.CPF]).Trim(), @"\d+").Value;
                var cargo = ObtemValorFormatoCorreto<string>(dr, ColunasArquivo.Cargo, validators[ColunasArquivo.Cargo]).Trim();
                var area = ObtemValorFormatoCorreto<string>(dr, ColunasArquivo.Area, validators[ColunasArquivo.Area]).Trim();
                var centroCusto = ObtemValorFormatoCorreto<string>(dr, ColunasArquivo.CentroCusto, validators[ColunasArquivo.CentroCusto]).Trim();
                var aplausogramas = ObtemValorFormatoCorreto<int>(dr, ColunasArquivo.Aplausogramas, validators[ColunasArquivo.Aplausogramas]);

                var associado = new AssociadoViewModel(
                    cpf, matricula, nome, cargo, area, centroCusto,
                    aplausogramas, EPerfilUsuario.Associado, Guid.Empty)
                {
                    GrupoNome = grupoNome
                };

                associados.Add(associado);
                linha++;

            }
            return associados;
        }

        private IEnumerable<Inconsistencia> RetornarInconsistenciasCPFsDuplicados(List<AssociadoViewModel> associados) =>
            associados.Select(e => e.Cpf).Distinct()
                .ToDictionary(cpf => cpf, cpf => associados.Count(e => e.Cpf == cpf))
                .Where(dic => dic.Value > 1)
                .Select(dic => new Inconsistencia(ColunasArquivo.CPF, 0, $"Há {dic.Value} linhas no arquivo com o CPF {dic.Key}."));
    }
}
