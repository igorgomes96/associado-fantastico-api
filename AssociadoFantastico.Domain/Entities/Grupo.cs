using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AssociadoFantastico.Domain.Entities
{
    public class Grupo: Entity
    {
        public Grupo() { } // EF
        public Grupo(string nome): base()
        {
            Nome = nome;
            Ativo = true;
        }

        public string Nome { get; private set; }
        public bool Ativo { get; private set; }

        private readonly List<Associado> _associados = new List<Associado>();
        public virtual IReadOnlyCollection<Associado> Associados => new ReadOnlyCollection<Associado>(_associados);

    }
}
