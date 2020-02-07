using AssociadoFantastico.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AssociadoFantastico.Domain.Entities
{
    public class Associado: Entity
    {
        public Associado() { } // EF

        public Associado(Usuario usuario, Grupo grupo, int aplausogramas, string centroCusto): base()
        {
            if (aplausogramas <= 0) throw new CustomException("A quantidade de aplausogramas deve ser maior que 0.");
            Aplausogramas = aplausogramas;
            Usuario = usuario ?? throw new CustomException("O usuário precisa ser informado.");
            UsuarioId = usuario.Id;
            Grupo = grupo ?? throw new CustomException("O grupo precisa ser informado.");
            GrupoId = grupo.Id;
            CentroCusto = centroCusto;
            Cargo = Usuario.Cargo;
            Area = Usuario.Area;
        }

        public string Cargo { get; private set; }
        public string Area { get; private set; }
        public int Aplausogramas { get; private set; }
        public string CentroCusto { get; private set; }
        public Guid UsuarioId { get; private set; }
        public Guid GrupoId { get; private set; }
        public Guid CicloId { get; private set; }

        public virtual Usuario Usuario { get; private set; }
        public virtual Grupo Grupo { get; private set; }
        public virtual Ciclo Ciclo { get; private set; }
        private readonly List<Elegivel> _elegiveis = new List<Elegivel>();
        public virtual IReadOnlyCollection<Elegivel> Elegiveis => new ReadOnlyCollection<Elegivel>(_elegiveis);

        public void AtualizarDados(Associado dadosAtualizados)
        {
            Cargo = dadosAtualizados.Cargo;
            Area = dadosAtualizados.Area;
            Aplausogramas = dadosAtualizados.Aplausogramas;
            Grupo = dadosAtualizados.Grupo ?? throw new CustomException("O grupo deve ser informado.");
            GrupoId = dadosAtualizados.GrupoId;
        }

    }
}
