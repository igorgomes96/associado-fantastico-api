using AssociadoFantastico.Domain.Enums;
using AssociadoFantastico.Domain.Exceptions;
using System;

namespace AssociadoFantastico.Domain.Entities
{
    public class Usuario: Entity
    {
        public Usuario() { }
        public Usuario(string cpf, string matricula, string nome, string cargo, string area, Empresa empresa): base()
        {
            Cpf = cpf;
            Matricula = matricula;
            Nome = nome;
            Cargo = cargo;
            Area = area;
            Empresa = empresa ?? throw new CustomException("A empresa precisa ser informada.");
            EmpresaId = empresa.Id;
        }

        public string Cpf { get; private set; }
        public string Matricula { get; private set; }
        public string Nome { get; private set; }
        public string Cargo { get; private set; }
        public string Area { get; private set; }
        public EPerfilUsuario Perfil { get; private set; }
        public Guid EmpresaId { get; private set; }
        
        public virtual Empresa Empresa { get; private set; }

        public void AtualizarDados(Usuario usuario)
        {
            Cpf = usuario.Cpf;
            Matricula = usuario.Matricula;
            Nome = usuario.Nome;
            Cargo = usuario.Cargo;
            Area = usuario.Area;
        }
    }
}
