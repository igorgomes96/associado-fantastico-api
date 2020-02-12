using AssociadoFantastico.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace AssociadoFantastico.Application.ViewModels
{
    public class UsuarioViewModel: EntityViewModel
    {
        //public UsuarioViewModel() { }
        //public UsuarioViewModel(string cpf, string matricula, string nome, string cargo, string area, EPerfilUsuario perfil)
        //{
        //    Cpf = cpf;
        //    Matricula = matricula;
        //    Nome = nome;
        //    Cargo = cargo;
        //    Area = area;
        //    Perfil = perfil;
        //}

        [Required(ErrorMessage = "O CPF do associado deve ser informado.")]
        [StringLength(11, ErrorMessage = "O CPF deve conter {1} caracteres.", MinimumLength = 11)]
        public string Cpf { get; set; }
        [Required(ErrorMessage = "A matrícula do associado deve ser informada.")]
        [StringLength(20, ErrorMessage = "A matrícula deve conter no máximo {1} caracteres.")]
        public string Matricula { get; set; }
        [Required(ErrorMessage = "O nome do associado deve ser informado.")]
        [StringLength(255, ErrorMessage = "O nome deve conter no máximo {1} caracteres.")]
        public string Nome { get; set; }
        [StringLength(255, ErrorMessage = "O cargo deve conter no máximo {1} caracteres.")]
        public string Cargo { get; set; }
        [StringLength(255, ErrorMessage = "A área deve conter no máximo {1} caracteres.")]
        public string Area { get; set; }
        public EPerfilUsuario Perfil { get; set; }
        public Guid EmpresaId { get; set; }
    }
}
