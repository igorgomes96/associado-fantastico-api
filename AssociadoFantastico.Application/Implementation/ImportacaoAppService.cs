using AssociadoFantastico.Application.EventsArgs;
using AssociadoFantastico.Application.Interfaces;
using AssociadoFantastico.Application.Repositories;
using AssociadoFantastico.Application.Services.Interfaces;
using AssociadoFantastico.Application.Services.Models;
using AssociadoFantastico.Application.ViewModels;
using AssociadoFantastico.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AssociadoFantastico.Application.Implementation
{
    public abstract class ImportacaoAppService : AppServiceBase<Importacao, ImportacaoViewModel>, IImportacaoAppService
    {
        protected readonly IExcelService _excelService;
        protected readonly DataColumnValidator[] _dataColumnValidators;
        protected readonly IProgressoImportacaoEvent _progressoEvent;
        protected const int LINHA_INICIAL_ARQUIVO = 1;
        protected const int QTDA_MAX_ERROS = 10;

        protected ImportacaoAppService(
            IUnitOfWork unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IImportacaoServiceConfiguration importacaoConfiguration,
            IProgressoImportacaoEvent progressoEvent) : base(unitOfWork, unitOfWork.ImportacaoRepository, mapper)
        {
            _excelService = excelService;
            _dataColumnValidators = importacaoConfiguration.Validators;
            _progressoEvent = progressoEvent;
        }

        protected abstract void ProcessarDados(Importacao importacao, DataTable dataTable);

        public void RealizarImportacaoEmBrackground()
        {
            Importacao importacao = (_repositoryBase as IImportacaoRepository).BuscarPrimeiraImportacaoPendenteDaFila();
            if (importacao == null) return;
            try
            {
                importacao.IniciarProcessamento();
                base.Atualizar(importacao);
                var arquivoImportacao = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), importacao.Arquivo);
                var dataTable = _excelService.LerTabela(arquivoImportacao, LINHA_INICIAL_ARQUIVO, 10);
                var inconsistencias = ValidarFormatoDataTable(dataTable);

                if (!FinalizarImportacaoComErro(importacao, inconsistencias))
                    ProcessarDados(importacao, dataTable);

            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                FinalizarImportacaoComErro(importacao, new[] { new Inconsistencia(string.Empty, 0, ex.Message) });
            }
        }

        protected void FinalizarImportacaoComSucesso(Importacao importacao)
        {
            importacao.FinalizarProcessamentoComSucesso();
            base.Atualizar(importacao);
            _progressoEvent.OnImportacaoFinalizada(this,
                new FinalizacaoImportacaoStatusEventArgs
                {
                    Status = StatusImportacao.FinalizadoComSucesso,
                    QtdaErros = 0,
                    CPFUsuario = importacao.CPFUsuarioImportacao
                });
        }


        protected virtual List<Inconsistencia> ValidarFormatoDataTable(DataTable dataTable)
        {
            var inconsistencias = new List<Inconsistencia>();

            // Valida se possui as colunas obrigatórias
            var obrigatorias = _dataColumnValidators.Where(v => v.Required);
            var colunasNaoEncontradas = ColunasObrigatoriasNaoEncontradas(dataTable, obrigatorias);
            if (colunasNaoEncontradas.Any())
                return colunasNaoEncontradas
                    .Select(coluna => new Inconsistencia(
                        coluna, LINHA_INICIAL_ARQUIVO,
                        $"A coluna {coluna} é obrigatória, porém não foi encontrada no arquivo.")).ToList();

            // Valida os valores
            int linha = LINHA_INICIAL_ARQUIVO + 1;
            foreach (DataRow dr in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    var validator = _dataColumnValidators.FirstOrDefault(v => v.ColumnName == dataTable.Columns[i].ColumnName);
                    if (validator == null) continue;
                    object value = dr[validator.ColumnName];
                    var erro = validator.ValidarValor(value);
                    if (!string.IsNullOrWhiteSpace(erro))
                        inconsistencias.Add(new Inconsistencia(validator.ColumnName, linha, erro));
                }
                var countLinhasErros = inconsistencias.Select(i => i.Linha).Distinct().Count();
                if (countLinhasErros > QTDA_MAX_ERROS)
                    break;
                linha++;
            }
            return inconsistencias;

        }

        private IEnumerable<string> ColunasObrigatoriasNaoEncontradas(DataTable dataTable, IEnumerable<DataColumnValidator> colunasObrigatorias)
        {
            foreach (var column in colunasObrigatorias)
                if (!dataTable.Columns.Contains(column.ColumnName))
                    yield return column.ColumnName;
        }

        protected IEnumerable<Inconsistencia> RetornarInconsistenciasDadosDuplicados(IEnumerable<object> dados, string coluna) =>
            dados.Distinct().ToDictionary(dado => dado, dado => dados.Count(d => d.Equals(dado)))
                .Where(dic => dic.Value > 1)
                .Select(dic => new Inconsistencia(coluna, 0, $"Há {dic.Value} linhas no arquivo com {coluna} {dic.Key}."));

        protected bool FinalizarImportacaoComErro(Importacao importacao, IEnumerable<Inconsistencia> inconsistencias)
        {
            if (inconsistencias.Any())
            {
                importacao.FinalizarImportacaoComFalha(inconsistencias);
                base.Atualizar(importacao);
                _progressoEvent.OnImportacaoFinalizada(this,
                    new FinalizacaoImportacaoStatusEventArgs
                    {
                        Status = StatusImportacao.FinalizadoComFalha,
                        QtdaErros = inconsistencias.Count(),
                        CPFUsuario = importacao.CPFUsuarioImportacao
                    });
                return true;
            }
            return false;
        }

        
        protected virtual T ObtemValorFormatoCorreto<T>(DataRow dr, string columnName, DataColumnValidator validator)
        {
            if (!dr.Table.Columns.Contains(columnName)) return default(T);
            return (T)validator.ParseValor(dr[columnName]);
        }

        protected void NotificarProgresso(int linhasProcessadas, int totalLinhas, string cpfUsuario)
        {
            _progressoEvent.OnNotificacaoProgresso(this, new ProgressoImportacaoEventArgs
            {
                EtapaAtual = 1,
                LinhasProcessadas = linhasProcessadas,
                TotalEtapas = 1,
                TotalLinhas = totalLinhas,
                EmailUsuario = cpfUsuario
            });
        }

        public IEnumerable<Inconsistencia> RetornarInconsistenciasDaImportacao(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
