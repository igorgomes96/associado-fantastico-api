using AssociadoFantastico.Domain.Exceptions;
using System;

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
        }

        public int Aplausogramas { get; private set; }
        public string CentroCusto { get; private set; }
        public Guid UsuarioId { get; private set; }
        public Guid GrupoId { get; private set; }
        public Guid CicloId { get; private set; }

        public virtual Usuario Usuario { get; private set; }
        public virtual Grupo Grupo { get; private set; }
        public virtual Ciclo Ciclo { get; private set; }

        public void AlterarGrupo(Grupo grupo)
        {
            throw new NotImplementedException();
        }

    }
}
